using System.Collections.Generic;
using AutoMapper;
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

            return CreatedAtRoute(nameof(GetTournamentById), new {Id = tournamentReadDto.Id}, tournamentReadDto); //create resource -> 201
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
    }
}