using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TennisApplication.Dtos.Match;
using TennisApplication.Dtos.Tournament;
using TennisApplication.Dtos.User;
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
            var tournamentDto = _mapper.Map<TournamentReadDto>(tournament);
            var usersDto = _mapper.Map<List<UserReadDto>>(_userRepository.GetUsersByTournament(id));
            var byeUser = _mapper.Map<UserReadDto>(_userRepository.GetByePlayer());

            TournamentCourse tournamentCourse = new TournamentCourse(usersDto, tournamentDto, byeUser);

            var matches = tournamentCourse.GetMatchesInCurrentRound()
                .Select(match => _mapper.Map<Match>(match)).ToList();
            for (int i = 0; i < matches.Count; i++)
            {
                matches[i].Tournament = tournament;
                _repository.DetachLocal(matches[i], matches[i].Id);
                _repository.SaveMatch(matches[i]);
                _repository.SaveChanges();
                tournamentCourse.UpdateIds(i, matches[i].Id);
            }

            return View("/Views/CourseTournament/CourseTournament.cshtml", tournamentCourse);
        }
        
        [HttpPost("/ongoing")]
        public IActionResult GetResultsTournament([FromForm] TournamentCourse tournamentCourse)
        {
            var tournamentId = int.Parse(Request.Form["TournamentId"]);
            Tournament tournament = _tournamentRepository.GetTournamentById(tournamentId);
            TournamentReadDto tournamentDto = _mapper.Map<TournamentReadDto>(tournament);
            
            if (tournamentCourse.Matches == null) //from /ongoingAll
            {
                tournamentCourse.TournamentDto = tournamentDto;
                tournamentCourse.Matches = _repository.GetMatchesByTournamentId(tournamentId)
                    .Select(match => _mapper.Map<MatchDto>(match)).ToList();
                tournamentCourse.updateOngoing();

                return View("/Views/CourseTournament/CourseTournament.cshtml", tournamentCourse);
            }
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
                
                MatchDto matchDto = _mapper.Map<MatchDto>(match);
                tournamentCourse.UpdateMatches(id, tournamentDto, matchDto.Player1, matchDto.Player2, match.Round, match.Winner, match.Result);

            }

            tournamentCourse.UpdateOthers(tournamentDto, round);
            if (!tournamentCourse.IsFinished)
            {
                var matches = tournamentCourse.GetMatchesInCurrentRound()
                    .Select(match => _mapper.Map<Match>(match)).ToList();

                foreach (var match in matches)
                {
                    match.Tournament = tournament;
                    _repository.DetachLocal(match, match.Id);
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
                return RedirectToAction(nameof(CompletedTournamentDetail), new {id = tournamentDto.Id} );
                
            }
            
            return View("/Views/CourseTournament/CourseTournament.cshtml", tournamentCourse);
        }

        [HttpGet("/completed")]
        public ActionResult CompletedTournamentView()
        {
            List<Tournament> tournaments = _tournamentRepository.GetCompletedTournaments();
            var tournamentReadDto = tournaments.Select(tournament => _mapper.Map<TournamentReadDto>(tournament));

            return View(tournamentReadDto);
        }

        [HttpGet("/results/{id}")]
        public ActionResult CompletedTournamentDetail(int id)
        {
            TournamentReadDto tournamentDto = _mapper.Map<TournamentReadDto>(_tournamentRepository.GetTournamentById(id));
            
            TournamentCourse tournamentCourse = new TournamentCourse(tournamentDto);
            List<Match> matches = _repository.GetMatchesByTournamentId(id);
            
            foreach (var match in matches)
            {
                match.Player1 =  _userRepository.GetUserById(match.Player1.Id);
                match.Player2 = _userRepository.GetUserById(match.Player2.Id);
            }

            tournamentCourse.Matches =  matches.Select(match => _mapper.Map<MatchDto>(match)).ToList();
            tournamentCourse.RoundsNumber = (int)Math.Ceiling(Math.Log2(tournamentDto.PlayersNumber));
            tournamentCourse.IsFinished = tournamentDto.Completed;
            tournamentCourse.CurrentRound = _repository.GetTournamentRound(tournamentDto.Id);
            
            return View(tournamentCourse);
        }
        [HttpGet("/ongoingAll")]
        public ActionResult OngoingAllView()
        {
            return View(_mapper.Map<IEnumerable<TournamentReadDto>>
                (_tournamentRepository.GetOngoingTournaments()));
        }
        
        
    }
}