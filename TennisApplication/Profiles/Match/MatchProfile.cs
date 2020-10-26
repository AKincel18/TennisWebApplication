using AutoMapper;
using TennisApplication.Dtos.Enrolment;
using TennisApplication.Dtos.Match;

namespace TennisApplication.Profiles.Match
{
    public class MatchProfile : Profile
    {
        public MatchProfile()
        {
            CreateMap<MatchDto, Models.Match>();
        }
    }
}