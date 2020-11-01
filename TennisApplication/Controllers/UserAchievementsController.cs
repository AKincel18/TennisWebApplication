using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TennisApplication.Dtos.Match;
using TennisApplication.Dtos.Tournament;
using TennisApplication.Dtos.User;
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
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("Index", "Home", new {area = ""}); 
            }
            
            IEnumerable<Tournament> tournaments =  _tournamentRepository.GetTournamentByUserId(loggedUser.Id);

            return View("/Views/Tournament/GetAllTournaments.cshtml",
                _mapper.Map<IEnumerable<TournamentReadDto>>(tournaments));
        }

        [HttpGet("/myMatches")]
        public ActionResult MyMatches()
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("Index", "Home", new {area = ""}); 
            }
            List<Match> matches = _matchRepository.GetMatchesByUserId(loggedUser.Id);
            return View(_mapper.Map<List<MatchDto>>(matches));
        }
    }
}