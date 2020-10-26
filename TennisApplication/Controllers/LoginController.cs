using System;
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

        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult LoginView()
        {
            return View();
        }
        
        [HttpPost]
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
    }
}