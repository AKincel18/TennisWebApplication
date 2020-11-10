using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TennisApplication.Dtos.Enrolment;
using TennisApplication.Dtos.User;
using TennisApplication.Models;
using TennisApplication.Others;
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
        
        [HttpGet("enrol/{id}")]
        public IActionResult EnrolTournament(int id)
        {            
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("LoginView", "Login", new {area = ""}); 
            }
            
            var enrolmentDto = new EnrolmentDto(id, loggedUser.Id);
            var enrolmentModel = _mapper.Map<Enrolment>(enrolmentDto);
            _repository.SaveEnrolment(enrolmentModel);
            _repository.SaveChanges();
            
            return RedirectToAction("GetIncomingTournament","Tournament");
            

        }

        [HttpGet("withdraw/{id}")]
        public IActionResult WithdrawTournament(int id)
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("LoginView", "Login", new {area = ""}); 
            }

            var enrolment = _repository.FindEnrolment(loggedUser.Id, id);
            _repository.DeleteEnrolment(enrolment);
            _repository.SaveChanges();

            return RedirectToAction("GetIncomingTournament","Tournament");
        }
    }
}