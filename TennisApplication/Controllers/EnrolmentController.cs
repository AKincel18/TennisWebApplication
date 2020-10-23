using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TennisApplication.Dtos.Enrolment;
using TennisApplication.Models;
using TennisApplication.Repository.Enrolment;

namespace TennisApplication.Controllers
{
    [Route("/enrol")]
    [ApiController]
    public class EnrolmentController : Controller
    {
        
        private readonly IEnrolmentRepository _repository;
        private readonly IMapper _mapper;

        public EnrolmentController(IEnrolmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        [HttpGet("{id}")]
        public IActionResult EnrolTournament(int id)
        {
            var enrolmentWriteDto = new EnrolmentWriteDto(id, LoggedUser.User.Id);
            var enrolmentModel = _mapper.Map<Enrolment>(enrolmentWriteDto);
            _repository.SaveEnrolment(enrolmentModel);
            _repository.SaveChanges();
            
            return RedirectToAction("GetIncomingTournament","Tournament");
        }
    }
}