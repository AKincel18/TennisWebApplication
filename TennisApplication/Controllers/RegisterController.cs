using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TennisApplication.Dtos.User;
using TennisApplication.Models;
using TennisApplication.Others;
using TennisApplication.Repository.User;

namespace TennisApplication.Controllers
{   
    [Route("/register")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public RegisterController(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult RegisterView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser([FromForm] UserCreateDto userCreateDto)
        {
            if (ModelState.IsValid)
            {
                LoggedUser.User = _mapper.Map<UserReadDto>(userCreateDto);
                
                var userModel = _mapper.Map<User>(userCreateDto);
                userModel.Password = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password); //hash password
                
                _repository.CreateUser(userModel);
                _repository.SaveChanges();
                
                return RedirectToAction("Index", "Home", new {area = ""});
            }

            Console.WriteLine("error");
            return BadRequest();

        }


    }
}