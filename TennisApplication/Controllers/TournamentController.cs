using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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

        public TournamentController(ITournamentRepository repository)
        {
            _repository = repository;
        }
        //private readonly TournamentRepositoryImpl _repository = new TournamentRepositoryImpl();
        
        //GET /tournament
        [HttpGet]
        public ActionResult<IEnumerable<Tournament>> GetAllTournaments()
        {
            var tournaments = _repository.GetAllTournaments();
            return Ok(tournaments);
        }
        
        //GET /tournament/{id}
        [HttpGet("{id}")]
        public ActionResult<Tournament> GetTournamentById(int id)
        {
            var tournament = _repository.GetTournamentById(id);
            return Ok(tournament);
        }
    }
}