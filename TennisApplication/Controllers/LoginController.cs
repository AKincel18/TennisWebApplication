using System.IO;
using System.Runtime.InteropServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;
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
            HttpContext.Session.SetString("SessionUser", JsonConvert.SerializeObject(null,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore}));
            return RedirectToAction("Index", "Home", new {area = ""});
        }

        [HttpPost("/in")]
        public ActionResult LoggedIn([FromForm] UserReadDto userReadDto)
        {
            var user = _repository.GetUserByEMail(userReadDto.EMail);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userReadDto.Password, user.Password))
            {
                ModelState.AddModelError("WrongEMailOrPassword", "Wrong e-mail or password");
                return View("/Views/Login/LoginView.cshtml", userReadDto);
            }

            UserReadDto toSaving = _mapper.Map<UserReadDto>(user);
            toSaving.AvatarPhoto();
            HttpContext.Session.SetString("SessionUser",JsonConvert.SerializeObject(toSaving));
            return RedirectToAction("Index", "Home", new {area = ""});
            
        }

        [HttpGet("/edit")]
        public ActionResult EditAccountView()
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            if (loggedUser == null)
            {
                return RedirectToAction("Index", "Home", new {area = ""}); 
            }
            User user = _repository.GetUserById(loggedUser.Id);
            return View(_mapper.Map<UserEditDto>(user));
        }

        [HttpPost("/editAction")]
        public ActionResult EditAccount([FromForm] UserEditDto userEditDto, IFormFile upload)
        {
            DeserializeUser deserializable = new DeserializeUser(new HttpContextAccessor());
            UserReadDto loggedUser = deserializable.GetLoggedUser();
            
            if (loggedUser != null)
            {
                User user = _repository.GetUserById(loggedUser.Id);
                user.FirstName = userEditDto.FirstName;
                user.LastName = userEditDto.LastName;
                user.EMail = userEditDto.EMail;
            
                if (userEditDto.Password != null)
                {
                    user.Password = userEditDto.Password;
                }
                
                
                if (upload != null)
                {
                    var image = Image.Load(upload.OpenReadStream());
                    image.Mutate(x => x.Resize(256, 256));
                    
                    //using var ms = new MemoryStream();
                    MemoryStream stream = new MemoryStream();
                    image.SaveAsPng(stream);
                    //var _IMemoryGroup = image.Ge
                    //var _MemoryGroup = _IMemoryGroup.ToArray()[0];
                    //var PixelData = MemoryMarshal.AsBytes(_MemoryGroup.Span).ToArray();
                    
                    //upload.CopyTo(ms);
                    user.Photo = stream.ToArray();
                    
                }
                
                _repository.SaveChanges();
                HttpContext.Session.SetString("SessionUser",JsonConvert.SerializeObject(_mapper.Map<UserReadDto>(user)));
            }
            
            return RedirectToAction("Index", "Home", new {area = ""});
        }
    }
}