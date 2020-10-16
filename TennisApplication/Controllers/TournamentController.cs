using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TennisApplication.Dtos;
using TennisApplication.Models;
using TennisApplication.Repository;

namespace TennisApplication.Controllers
{
    //[Route("/[controller]")]
    [Route("/tournaments")]
    [ApiController]
    public class TournamentController : Controller
    {
        private readonly ITournamentRepository _repository;
        private readonly IMapper _mapper;

        public TournamentController(ITournamentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        //GET /tournaments
        [HttpGet]
        public ActionResult<IEnumerable<TournamentReadDto>> GetAllTournaments()
        {
            var tournaments = _repository.GetAllTournaments();
            return Ok(_mapper.Map<IEnumerable<TournamentReadDto>>(tournaments));
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
        public ActionResult<TournamentReadDto> CreateTournament(TournamentCreateDto tournamentCreateDto)
        {
            var tournamentModel = _mapper.Map<Tournament>(tournamentCreateDto);
            _repository.CreateTournament(tournamentModel);
            _repository.SaveChanges();

            var tournamentReadDto = _mapper.Map<TournamentReadDto>(tournamentModel);

            return CreatedAtRoute(nameof(GetTournamentById), new {tournamentReadDto.Id}, tournamentReadDto); //create resource -> 201
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

            return NoContent();
        }
    }
}