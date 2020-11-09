using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TennisApplication.Dtos.User;
using TennisApplication.Models;
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
        public ActionResult CreateUser([FromForm] UserCreateDto userCreateDto, IFormFile  upload)
        {
            if (upload != null)
            {
                using var ms = new MemoryStream();
                upload.CopyTo(ms);
                userCreateDto.Photo = ms.ToArray();
            }


            User user = _repository.GetUserByEMail(userCreateDto.EMail);
            if (user != null) //the same e-mail
            {
                ModelState.AddModelError("EMailExists", "E-Mail already exists");
                return View("/Views/Register/RegisterView.cshtml", userCreateDto);
            }


            var userModel = _mapper.Map<User>(userCreateDto);
            userModel.Password = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password); //hash password

            _repository.CreateUser(userModel);
            _repository.SaveChanges();

            UserReadDto userSession = _mapper.Map<UserReadDto>(userModel);
            userSession.AvatarPhoto();
            HttpContext.Session.SetString("SessionUser",JsonConvert.SerializeObject(userSession));

            return RedirectToAction("Index", "Home", new {area = ""});

        }


    }
}