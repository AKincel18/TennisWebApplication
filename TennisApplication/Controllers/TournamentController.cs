using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TennisApplication.Dtos.Enrolment;
using TennisApplication.Dtos.Match;
using TennisApplication.Dtos.Tournament;
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
        public ActionResult<TournamentReadDto> CreateTournament([FromForm]TournamentCreateDto tournamentCreateDto)
        {
            var tournamentModel = _mapper.Map<Tournament>(tournamentCreateDto);
            _repository.CreateTournament(tournamentModel);
            _repository.SaveChanges();

            var tournamentReadDto = _mapper.Map<TournamentReadDto>(tournamentModel);
            
            //return CreatedAtRoute(nameof(GetTournamentById), new {tournamentReadDto.Id}, tournamentReadDto); //create resource -> 201
            return RedirectToAction(nameof(GetAllTournaments)); //return to GetAllTournaments
        }
        
        //PUT /tournaments/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateTournament(int id, TournamentCreateDto tournamentCreateDto)
        {
            var tournamentModelFromRepository = _repository.GetTournamentById(id);
            if (tournamentModelFromRepository == null)
            {
                return NotFound();
            }

            _mapper.Map(tournamentCreateDto, tournamentModelFromRepository); //updating
            
            //_repository.UpdateTournament(tournamentModelFromRepository);
            _repository.SaveChanges();

            return NoContent();
        }
        
        //PATCH /tournaments/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialTournamentUpdate(int id, JsonPatchDocument<TournamentCreateDto> patchDocument)
        {
            var tournamentModelFromRepository = _repository.GetTournamentById(id);
            if (tournamentModelFromRepository == null)
            {
                return NotFound();
            }

            var tournamentToPatch = _mapper.Map<TournamentCreateDto>(tournamentModelFromRepository);
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
        [HttpDelete("{id}")]
        public ActionResult DeleteTournament(int id)
        {
            var tournamentModelFromRepository = _repository.GetTournamentById(id);
            if (tournamentModelFromRepository == null)
            {
                return NotFound();
            }
            _repository.DeleteTournament(tournamentModelFromRepository);
            _repository.SaveChanges();

            return RedirectToAction(nameof(GetAllTournaments)); //return to GetAllTournaments
        }

        [HttpGet("/incoming")]
        public ActionResult GetIncomingTournament()
        {
            var tournaments = _repository.GetIncomingTournament();
            List<TournamentUserReadDto> tournamentUserReadDtos = new List<TournamentUserReadDto>();
            
            foreach (var tournament in tournaments)
            {
                var users = _userRepository.GetUsersByTournament(tournament.Id);
                var isRegistered = _userRepository.IsUserRegisteredForTournamentById(LoggedUser.User.Id, tournament.Id);
                tournamentUserReadDtos.Add(new TournamentUserReadDto(tournament, users, isRegistered));
            }
            
            //return View((_mapper.Map<IEnumerable<TournamentReadDto>>(tournaments)));
            return View(tournamentUserReadDtos);
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
            
            if (tournamentCourse == null) //start tournament
            {
                var tournament = _repository.GetTournamentById(id);
                var users = _userRepository.GetUsersByTournament(id);

                tournamentCourse = new TournamentCourse(users, tournament);
                tournamentCourse.DrawFirstRound();

                var matches = new List<Match>();
                foreach (var match in tournamentCourse.Matches)
                {
                    matches.Add(_mapper.Map<Match>(match));
                }

                for (int i=0; i < matches.Count; i++)
                {
                
                    _matchRepository.SaveMatch(matches[i]);
                    _matchRepository.SaveChanges();
                
                    tournamentCourse.UpdateIds(i, matches[i].Id);
                }
            }

            
            return View(tournamentCourse);
        }
        
        [HttpPost("/ongoing")]
        public IActionResult GetResultsTournament([FromForm] TournamentCourse tournamentCourse)
        {
            var tournamentId = int.Parse(Request.Form["TournamentId"]);
            Tournament tournament = _repository.GetTournamentById(tournamentId);
            int round = 0;
            int numberOfMatches = tournamentCourse.Matches.Count;
            
            for (int i = 0; i < numberOfMatches; i++)
            {
                int id = int.Parse(Request.Form["MatchDto[" + i + "].Id"]);
                
                Match match = _matchRepository.GetMatchById(id);
                match.Result = tournamentCourse.Matches[i].Result;
                match.Winner = tournamentCourse.Matches[i].Winner;
                _matchRepository.SaveChanges();
                
                int p1 = int.Parse(Request.Form["MatchDto[" + i + "].Player1"]);
                int p2 = int.Parse(Request.Form["MatchDto[" + i + "].Player2"]);
                User player1 = _userRepository.GetUserById(p1);
                User player2 = _userRepository.GetUserById(p2);
                //string tournament = Request.Form["MatchDto[" + i + "].Tournament"];
                round = int.Parse(Request.Form["MatchDto[" + i + "].Round"]);
                tournamentCourse.UpdateMatches(id, tournament, player1, player2, round, match.Winner, match.Result);
                

            }

            tournamentCourse.UpdateOthers(tournament, round);
            TempData["Model"] = JsonConvert.SerializeObject(tournamentCourse);
            
            //return Ok();
            return RedirectToAction(nameof(StartTournament), new {id = tournament.Id} );
        }

        
        
        
    }
}