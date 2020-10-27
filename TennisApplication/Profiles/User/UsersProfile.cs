﻿using AutoMapper;
using TennisApplication.Dtos.User;

namespace TennisApplication.Profiles.User
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<UserCreateDto, Models.User>();
            CreateMap<UserCreateDto, UserReadDto>();
        }

    }
}