using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TennisApplication.Dtos.Enrolment;
using TennisApplication.Dtos.Match;

namespace TennisApplication.Profiles.Match
{
    public class MatchProfile : Profile
    {
        public MatchProfile()
        {
            CreateMap<MatchDto, Models.Match>();
            CreateMap<Models.Match, MatchDto>();
        }
    }
}