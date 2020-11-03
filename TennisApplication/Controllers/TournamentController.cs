using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TennisApplication.Dtos.Enrolment;
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
            var tournaments = _repository.GetAllTournaments();
            return View((_mapper.Map<IEnumerable<TournamentReadDto>>(tournaments)));
            //return RedirectToPage("./GetAllTournaments", _mapper.Map<IEnumerable<TournamentReadDto>>(tournaments));
        }

        [HttpGet("/createTournament")]
        public ActionResult CreateTournamentView()
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser != null && loggedUser.Role == Role.TournamentDirector)
            {
                return View();
            }
            return RedirectToAction("Index", "Home", new {area = ""}); 
            
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
        public ActionResult<TournamentReadDto> CreateTournament([FromForm]TournamentCreateDto tournamentCreateDto)
        {
            Tournament tournament;
            if (tournamentCreateDto.Id != 0)
            {
                tournament = _repository.GetTournamentById(tournamentCreateDto.Id);
                tournament.Name = tournamentCreateDto.Name;
                tournament.Date = tournamentCreateDto.Date;
                tournament.Place = tournamentCreateDto.Place;
                tournament.PlayersNumber = tournamentCreateDto.PlayersNumber;
            }
            else
            {
                tournament = _mapper.Map<Tournament>(tournamentCreateDto);
                _repository.CreateTournament(tournament);
            }
            _repository.SaveChanges();

            //var tournamentReadDto = _mapper.Map<TournamentReadDto>(tournamentModel);
            
            //return CreatedAtRoute(nameof(GetTournamentById), new {tournamentReadDto.Id}, tournamentReadDto); //create resource -> 201
            return RedirectToAction(nameof(GetAllTournaments)); //return to GetAllTournaments
        }
        
        //PUT /tournaments/{id}
        [HttpGet("/edit/{id}")]
        public ActionResult UpdateTournamentView(int id /*[FromForm] TournamentCreateDto tournamentCreateDto*/)
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null || loggedUser.Role == Role.Player)
            {
                return RedirectToAction("Index", "Home", new {area = ""}); 
            }
            var tournamentModelFromRepository = _repository.GetTournamentById(id);
            if (tournamentModelFromRepository == null)
            {
                return NotFound();
            }

            TournamentCreateDto tournamentCreateDto = _mapper.Map<TournamentCreateDto>(tournamentModelFromRepository);
            
            //_mapper.Map(tournamentCreateDto, tournamentModelFromRepository); //updating
            
            //_repository.UpdateTournament(tournamentModelFromRepository);
            //_repository.SaveChanges();

            return View("CreateTournamentView", tournamentCreateDto);
        }
        
        //PATCH /tournaments/{id}
        [HttpPatch]
        public ActionResult PartialTournamentUpdate(int id, [FromForm] JsonPatchDocument<TournamentReadDto> patchDocument)
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
            if (loggedUser == null || loggedUser.Role == Role.Player)
            {
                return RedirectToAction("Index", "Home", new {area = ""}); 
            }
            
            var tournamentModelFromRepository = _repository.GetTournamentById(id);
            if (tournamentModelFromRepository == null)
            {
                return NotFound();
            }

            if (_matchRepository.IsAnyMatchInTheTournament(id))
            {
                ModelState.AddModelError("CantDelete", "Cannot delete tournament: '" + 
                                                       tournamentModelFromRepository.Name + "' with matches!");
                //return RedirectToAction(nameof(GetAllTournaments));
                return View("GetAllTournaments", _mapper.Map<IEnumerable<TournamentReadDto>>(_repository.GetAllTournaments()));
            }
            
            _repository.DeleteTournament(tournamentModelFromRepository);
            _repository.SaveChanges();

            return RedirectToAction(nameof(GetAllTournaments)); //return to GetAllTournaments
        }

        [HttpGet("/incoming")]
        public ActionResult GetIncomingTournament()
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            /*if (loggedUser == null)
            {
                return RedirectToAction("Index", "Home", new {area = ""}); 
            }*/
            
            var tournaments = _repository.GetIncomingTournament();
            List<TournamentUserReadDto> tournamentUserReadDtos = new List<TournamentUserReadDto>();
            
            foreach (var tournament in tournaments)
            {
                var users = _userRepository.GetUsersByTournament(tournament.Id);
                var isRegistered = loggedUser != null && _userRepository.IsUserRegisteredForTournamentById(loggedUser.Id,
                    tournament.Id);
                
                if (!_matchRepository.IsAnyMatchInTheTournament(tournament.Id))
                {
                    tournamentUserReadDtos.Add(new TournamentUserReadDto(tournament, users, isRegistered));
                }
            }
            
            //return View((_mapper.Map<IEnumerable<TournamentReadDto>>(tournaments)));
            return View(tournamentUserReadDtos);
        }
        

    }
}