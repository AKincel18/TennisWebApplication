using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TennisApplication.Dtos.Match;
using TennisApplication.Dtos.Tournament;
using TennisApplication.Models;
using TennisApplication.Others;
using TennisApplication.Repository.Match;
using TennisApplication.Repository.Tournament;

namespace TennisApplication.Controllers
{
    [ApiController]
    public class UserAchievementsController : Controller
    {        
        private readonly IMapper _mapper;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMatchRepository _matchRepository;

        public UserAchievementsController(IMapper mapper, ITournamentRepository tournamentRepository, IMatchRepository matchRepository)
        {
            _mapper = mapper;
            _tournamentRepository = tournamentRepository;
            _matchRepository = matchRepository;
        }

        [HttpGet("/myTournaments")]
        public ActionResult MyTournaments()
        {
            IEnumerable<Tournament> tournaments =  _tournamentRepository.GetTournamentByUserId(LoggedUser.User.Id);

            return View("/Views/Tournament/GetAllTournaments.cshtml",
                _mapper.Map<IEnumerable<TournamentReadDto>>(tournaments));
        }

        [HttpGet("/myMatches")]
        public ActionResult MyMatches()
        {
            List<Match> matches = _matchRepository.GetMatchesByUserId(LoggedUser.User.Id);
            return View(_mapper.Map<List<MatchDto>>(matches));
        }
    }
}