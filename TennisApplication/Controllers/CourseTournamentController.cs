using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TennisApplication.Dtos.Match;
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
            TournamentCourse tournamentCourse;
            try
            {
                tournamentCourse = JsonConvert.DeserializeObject<TournamentCourse>((string) TempData["Model"]);
            }
            catch (ArgumentNullException)
            {
                tournamentCourse = null;
            }

            var tournament = _tournamentRepository.GetTournamentById(id);
            if (tournamentCourse == null) //start tournament
            {
                var users = _userRepository.GetUsersByTournament(id);

                tournamentCourse = new TournamentCourse(users, tournament);
                tournamentCourse.DrawFirstRound();
                
                var matchesInRound = tournamentCourse.Matches
                    .Where(m => m.Round == tournamentCourse.CurrentRound)
                    .ToList();

                //tournamentCourse.SetTournament(tournament, matchesInRound); //error when tournament from tempData (must be from database)
            
                var matches = matchesInRound.Select(match => _mapper.Map<Match>(match)).ToList();
                for (int i = 0; i < matches.Count; i++)
                {
                    _repository.SaveMatch(matches[i]);
                    _repository.SaveChanges();

                    tournamentCourse.UpdateIds(i, matches[i].Id);
                }
                
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
                
                int p1 = int.Parse(Request.Form["MatchDto[" + i + "].Player1"]);
                int p2 = int.Parse(Request.Form["MatchDto[" + i + "].Player2"]);
                User player1 = _userRepository.GetUserById(p1);
                User player2 = _userRepository.GetUserById(p2);
                //string tournament = Request.Form["MatchDto[" + i + "].Tournament"];
                round = int.Parse(Request.Form["MatchDto[" + i + "].Round"]);
                tournamentCourse.UpdateMatches(id, tournament, player1, player2, round, match.Winner, match.Result);

            }

            tournamentCourse.UpdateOthers(tournament, round);
            if (!tournamentCourse.isFinished)
            {
                //todo duplicates code
                var matchesInRound = tournamentCourse.Matches
                    .Where(m => m.Round == tournamentCourse.CurrentRound)
                    .ToList();

                var matches = matchesInRound.Select(match => _mapper.Map<Match>(match)).ToList();

                foreach (var match in matches)
                {
                    _repository.SaveMatch(match);
                    _repository.SaveChanges();
                    MatchDto matchDto = _mapper.Map<MatchDto>(match);
                    tournamentCourse.Matches.Add(matchDto);
                }
            
                tournamentCourse.Matches.RemoveAll(m => m.Id == 0);
            }
            
            TempData["Model"] = JsonConvert.SerializeObject(tournamentCourse);
            
            //return Ok();
            return RedirectToAction(nameof(StartTournament), new {id = tournament.Id} );
        }
    }
}