using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TennisApplication.Dtos.User;
using TennisApplication.Models;
using TennisApplication.Others;
using TennisApplication.Repository.User;

namespace TennisApplication.Controllers
{    
    [Route("/login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public LoginController(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult LoginView()
        {
            return View();
        }
        
        [HttpGet("/out")]
        public ActionResult LoggedOut()
        {
            LoggedUser.User = null;
            return RedirectToAction("Index", "Home", new {area = ""});
        }

        [HttpPost("/in")]
        public ActionResult LoggedIn([FromForm] UserReadDto userReadDto)
        {
            var users = _repository.GetAllUsers();
            foreach (var user in users)
            {
                if (user.EMail.Equals(userReadDto.EMail) &&
                    user.Password.Equals(userReadDto.Password))
                {
                    User foundUser = _repository.GetUserById(_repository.FindIdByEMailAndPassword(
                        userReadDto.EMail,
                        userReadDto.Password));
                    
                    userReadDto.Role = foundUser.Role;
                    userReadDto.FirstName = foundUser.FirstName;
                    userReadDto.Id = foundUser.Id;
                    LoggedUser.User = userReadDto;
                    
                    Console.WriteLine("logged: " + userReadDto.FirstName);
                    return RedirectToAction("Index", "Home", new {area = ""});
                }
            }

            Console.WriteLine("not found user with given parameters");
            return BadRequest();
        }

        [HttpGet("/edit")]
        public ActionResult EditAccountView()
        {
            User user = _repository.GetUserById(LoggedUser.User.Id);
            return View(_mapper.Map<UserEditDto>(user));
        }

        [HttpPost("/editAction")]
        public ActionResult EditAccount([FromForm] UserEditDto userEditDto)
        {
            User user = _repository.GetUserById(LoggedUser.User.Id);
            user.FirstName = userEditDto.FirstName;
            user.LastName = userEditDto.LastName;
            user.EMail = userEditDto.EMail;
            
            if (userEditDto.Password != null)
            {
                user.Password = userEditDto.Password;
            }
            
            _repository.SaveChanges();
            return RedirectToAction("Index", "Home", new {area = ""});
        }
    }
}