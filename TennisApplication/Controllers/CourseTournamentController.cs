using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TennisApplication.Dtos.Match;
using TennisApplication.Dtos.Tournament;
using TennisApplication.Models;
using TennisApplication.Others;
using TennisApplication.Repository.Match;
using TennisApplication.Repository.Tournament;
using TennisApplication.Repository.User;

namespace TennisApplication.Controllers
{
    [Route("/course")]
    [ApiController]
    public class CourseTournamentController : Controller
    {
        private readonly IMatchRepository _repository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CourseTournamentController(IMatchRepository repository, ITournamentRepository tournamentRepository, 
            IUserRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _tournamentRepository = tournamentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("/ongoing/{id}")]
        public IActionResult StartTournament(int id)
        {
            var tournament = _tournamentRepository.GetTournamentById(id);
            var users = _userRepository.GetUsersByTournament(id);
            var byeUser = _userRepository.GetByePlayer();

            TournamentCourse tournamentCourse = new TournamentCourse(users, tournament, byeUser);

            var matches = tournamentCourse.GetMatchesInCurrentRound()
                .Select(match => _mapper.Map<Match>(match)).ToList();
            for (int i = 0; i < matches.Count; i++)
            {
                _repository.SaveMatch(matches[i]);
                _repository.SaveChanges();
                tournamentCourse.UpdateIds(i, matches[i].Id);
            }

            return View("/Views/Tournament/StartTournament.cshtml", tournamentCourse);
        }
        
        [HttpPost("/ongoing")]
        public IActionResult GetResultsTournament([FromForm] TournamentCourse tournamentCourse)
        {
            var tournamentId = int.Parse(Request.Form["TournamentId"]);
            Tournament tournament = _tournamentRepository.GetTournamentById(tournamentId);
            int round = 0;
            int numberOfMatches = tournamentCourse.Matches.Count;
            for (int i = 0; i < numberOfMatches; i++)
            {
                int id = int.Parse(Request.Form["MatchDto[" + i + "].Id"]);
                
                Match match = _repository.GetMatchById(id);
                match.Result = tournamentCourse.Matches[i].Result;
                match.Winner = tournamentCourse.Matches[i].Winner;
                
                _repository.SaveChanges(); //update finished matches
                round = match.Round; //last round 
                tournamentCourse.UpdateMatches(id, tournament, match.Player1, match.Player2, match.Round, match.Winner, match.Result);

            }

            tournamentCourse.UpdateOthers(tournament, round);
            if (!tournamentCourse.IsFinished)
            {
                var matches = tournamentCourse.GetMatchesInCurrentRound()
                    .Select(match => _mapper.Map<Match>(match)).ToList();

                foreach (var match in matches)
                {
                    _repository.SaveMatch(match);
                    _repository.SaveChanges();
                    MatchDto matchDto = _mapper.Map<MatchDto>(match);
                    tournamentCourse.Matches.Add(matchDto);
                }
            
                tournamentCourse.Matches.RemoveAll(m => m.Id == 0);
            }
            else
            {
                tournament.Completed = true;
                _tournamentRepository.SaveChanges();
                return RedirectToAction(nameof(CompletedTournamentDetail), new {id = tournament.Id} );
                
            }
            
            return View("/Views/Tournament/StartTournament.cshtml", tournamentCourse);
        }

        [HttpGet("/completed")]
        public ActionResult CompletedTournamentView()
        {
            List<Tournament> tournaments = _tournamentRepository.GetCompletedTournaments();
            var tournamentReadDtos = tournaments.Select(tournament => _mapper.Map<TournamentReadDto>(tournament));

            return View(tournamentReadDtos);
        }

        [HttpGet("/completed/{id}")]
        public ActionResult CompletedTournamentDetail(int id)
        {
            Tournament tournament = _tournamentRepository.GetTournamentById(id);
            
            TournamentCourse tournamentCourse = new TournamentCourse(tournament);
            List<Match> matches = _repository.GetMatchesByTournamentId(id);
            
            foreach (var match in matches)
            {
                match.Player1 =  _userRepository.GetUserById(match.Player1.Id);
                match.Player2 = _userRepository.GetUserById(match.Player2.Id);
            }

            tournamentCourse.Matches =  matches.Select(match => _mapper.Map<MatchDto>(match)).ToList();
            tournamentCourse.RoundsNumber = (int)Math.Ceiling(Math.Log2(tournament.PlayersNumber));
            
            return View(tournamentCourse);
        }
    }
}