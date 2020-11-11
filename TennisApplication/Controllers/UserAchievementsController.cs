using System;
using System.Collections.Generic;
using System.Linq;
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
using TennisApplication.Repository.User;

namespace TennisApplication.Controllers
{
    [ApiController]
    public class UserAchievementsController : Controller
    {        
        private readonly IMapper _mapper;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IUserRepository _userRepository;

        public UserAchievementsController(IMapper mapper, ITournamentRepository tournamentRepository, IMatchRepository matchRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _tournamentRepository = tournamentRepository;
            _matchRepository = matchRepository;
            _userRepository = userRepository;
        }

        [HttpGet("/myTournaments")]
        public ActionResult MyTournaments()
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("LoginView", "Login", new {area = ""}); 
            }

            if (loggedUser.Role == Role.TournamentDirector)
            {
                return RedirectToAction("GetAllTournaments", "Tournament", new {area = ""}); 
            }
            
            List<TournamentReadDto> tournaments =  _mapper.Map<List<TournamentReadDto>>(
                _tournamentRepository.GetTournamentByUserId(loggedUser.Id).ToList());
            foreach (var tournament in tournaments.Where(tournament => !tournament.Completed))
            {
                tournament.Started = _tournamentRepository.IsTournamentStarted(tournament.Id);
            }
            return tournaments.Any()
                ? View("/Views/Tournament/GetAllTournaments.cshtml",
                    _mapper.Map<IEnumerable<TournamentReadDto>>(tournaments))
                : View("NoTournaments", loggedUser);
        }

        [HttpGet("/matches/{id}")]
        public ActionResult PlayerMatches(int id)
        {
            var matches = _matchRepository.GetMatchesByUserId(id);
            var player = _mapper.Map<UserReadDto>(_userRepository.GetUserById(id));
            
            var matchesDto = _mapper.Map<List<MatchDto>>(matches);
            
            //problem with autoMapper (not mapping tournament)
            for (int i = 0; i < matchesDto.Count; i++)
            {
                matchesDto[i].TournamentDto = _mapper.Map<TournamentReadDto>(matches[i].Tournament);
            }

            return matchesDto.Count == 0 ? View("NoMatches", player) : View(new UserMatchesDto(matchesDto, player));
        }

        [HttpGet("/myMatches")]
        public IActionResult MyMatches()
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("LoginView", "Login", new {area = ""}); 
            }

            if (loggedUser.Role == Role.TournamentDirector)
            {
                return RedirectToAction("GetAllTournaments", "Tournament", new {area = ""}); 
            }
            
            var matches = _matchRepository.GetMatchesByUserId(loggedUser.Id);
            var matchesDto = _mapper.Map<List<MatchDto>>(matches);
            
            //problem with autoMapper (not mapping tournament)
            for (int i = 0; i < matchesDto.Count; i++)
            {
                matchesDto[i].TournamentDto = _mapper.Map<TournamentReadDto>(matches[i].Tournament);
            }

            return matchesDto.Count == 0 ? 
                View("NoMatches", loggedUser) : View("/Views/UserAchievements/PlayerMatches.cshtml",new UserMatchesDto(matchesDto, loggedUser));
        }
    }
}