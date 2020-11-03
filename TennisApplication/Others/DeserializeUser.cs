using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TennisApplication.Dtos.User;


namespace TennisApplication.Others
{
    public class DeserializeUser
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public DeserializeUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public UserReadDto GetLoggedUser()
        {
            UserReadDto loggedUser;
            try
            {
                loggedUser =
                    JsonConvert.DeserializeObject<UserReadDto>(_contextAccessor.HttpContext.Session.GetString("SessionUser"));
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            
            return loggedUser;
        }

    }
}