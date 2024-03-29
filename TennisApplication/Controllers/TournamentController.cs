﻿using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TennisApplication.Dtos.Tournament;
using TennisApplication.Dtos.User;
using TennisApplication.Models;
using TennisApplication.Others;
using TennisApplication.Repository.Match;
using TennisApplication.Repository.Tournament;
using TennisApplication.Repository.User;

namespace TennisApplication.Controllers
{
    //[Route("/[controller]")]
    [Route("/tournaments")]
    [ApiController]
    public class TournamentController : Controller
    {
        private readonly ITournamentRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IMapper _mapper;

        public TournamentController(ITournamentRepository repository, IMapper mapper,
            IUserRepository userRepository, IMatchRepository matchRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _matchRepository = matchRepository;
        }


        //GET /tournaments
        [HttpGet]
        public ActionResult<IEnumerable<TournamentReadDto>> GetAllTournaments()
        {
            var ongoing = _mapper.Map<IEnumerable<TournamentReadDto>>(_repository.GetOngoingTournaments());
            var notStarted = _mapper.Map<IEnumerable<TournamentReadDto>>(_repository.GetNotStartedTournaments());
            var completed = _mapper.Map<IEnumerable<TournamentReadDto>>(_repository.GetCompletedTournaments());
            var all = new List<TournamentReadDto>();
            foreach (var tournamentReadDto in ongoing)
            {
                tournamentReadDto.Started = true;
                all.Add(tournamentReadDto);
            }

            foreach (var tournamentReadDto in notStarted)
            {
                tournamentReadDto.Started = false;
                all.Add(tournamentReadDto);
            }

            foreach (var tournamentReadDto in completed)
            {
                tournamentReadDto.Started = true;
                all.Add(tournamentReadDto);
            }

            return View(all);
        }

        [HttpGet("/createTournament")]
        public ActionResult CreateTournamentView()
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("LoginView", "Login");
            }

            if (loggedUser.Role == Role.Player)
            {
                return RedirectToAction("GetAllTournaments", "Tournament", new {area = ""});
            }

            return View();
        }

        //GET /tournaments/{id}
        [HttpGet("{id}", Name = "GetTournamentById")]
        public ActionResult<TournamentReadDto> GetTournamentById(int id)
        {
            var tournament = _repository.GetTournamentById(id);
            if (tournament != null)
            {
                return Ok(_mapper.Map<TournamentReadDto>(tournament));
            }

            return NotFound();
        }

        //POST /tournaments
        [HttpPost]
        public ActionResult<TournamentReadDto> CreateTournament([FromForm] TournamentCreateDto tournamentCreateDto)
        {
            Tournament tournament;
            if (tournamentCreateDto.Id != 0)
            {
                tournament = _repository.GetTournamentById(tournamentCreateDto.Id);
                tournament.Name = tournamentCreateDto.Name;
                tournament.Date = tournamentCreateDto.Date;
                tournament.Place = tournamentCreateDto.Place;
                tournament.DrawSize = tournamentCreateDto.DrawSize;
            }
            else
            {
                tournament = _mapper.Map<Tournament>(tournamentCreateDto);
                _repository.CreateTournament(tournament);
            }

            _repository.SaveChanges();
            return RedirectToAction(nameof(GetAllTournaments)); //return to GetAllTournaments
        }

        //PUT /tournaments/{id}
        [HttpGet("/edit/{id}")]
        public ActionResult UpdateTournamentView(int id)
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("LoginView", "Login", new {area = ""});
            }

            if (loggedUser.Role == Role.Player)
            {
                return RedirectToAction("GetAllTournaments", "Tournament", new {area = ""});
            }

            var tournamentModelFromRepository = _repository.GetTournamentById(id);
            if (tournamentModelFromRepository == null)
            {
                return NotFound();
            }

            TournamentCreateDto tournamentCreateDto = _mapper.Map<TournamentCreateDto>(tournamentModelFromRepository);
            return View("CreateTournamentView", tournamentCreateDto);
        }

        //PATCH /tournaments/{id}
        [HttpPatch]
        public ActionResult PartialTournamentUpdate(int id,
            [FromForm] JsonPatchDocument<TournamentReadDto> patchDocument)
        {
            var tournamentModelFromRepository = _repository.GetTournamentById(id);
            if (tournamentModelFromRepository == null)
            {
                return NotFound();
            }

            var tournamentToPatch = _mapper.Map<TournamentReadDto>(tournamentModelFromRepository);
            patchDocument.ApplyTo(tournamentToPatch, ModelState);

            if (!TryValidateModel(tournamentToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(tournamentToPatch, tournamentModelFromRepository); //patching
            _repository.SaveChanges();

            return NoContent();
        }

        //DELETE /tournaments/{id}
        [HttpGet("/delete/{id}")]
        public ActionResult DeleteTournament(int id)
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("LoginView", "Login", new {area = ""});
            }

            if (loggedUser.Role == Role.Player)
            {
                return RedirectToAction("GetAllTournaments", "Tournament", new {area = ""});
            }

            var tournamentModelFromRepository = _repository.GetTournamentById(id);
            if (tournamentModelFromRepository == null)
            {
                return NotFound();
            }

            if (_matchRepository.IsAnyMatchInTheTournament(id))
            {
                TempData["CantDelete"] = tournamentModelFromRepository.Name;
                return RedirectToAction(nameof(GetAllTournaments));
            }

            _repository.DeleteTournament(tournamentModelFromRepository);
            _repository.SaveChanges();
            TempData["deleted"] = tournamentModelFromRepository.Name;
            return RedirectToAction(nameof(GetAllTournaments)); //return to GetAllTournaments
        }

        [HttpGet("/incoming")]
        public ActionResult GetIncomingTournament()
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();

            IEnumerable<TournamentReadDto> tournamentsReadDto =
                _mapper.Map<IEnumerable<TournamentReadDto>>(_repository.GetIncomingTournament());

            List<TournamentParticipants> tournamentUserReadDtos = new List<TournamentParticipants>();

            foreach (var tournamentDto in tournamentsReadDto)
            {
                var usersDto = _mapper.Map<List<UserReadDto>>(_userRepository.GetUsersByTournament(tournamentDto.Id));
                var isRegistered = loggedUser != null && _userRepository.IsUserRegisteredForTournamentById(
                    loggedUser.Id,
                    tournamentDto.Id);

                if (!_matchRepository.IsAnyMatchInTheTournament(tournamentDto.Id))
                {
                    tournamentUserReadDtos.Add(new TournamentParticipants(tournamentDto, usersDto, isRegistered));
                }
            }

            return View(tournamentUserReadDtos);
        }
    }
}