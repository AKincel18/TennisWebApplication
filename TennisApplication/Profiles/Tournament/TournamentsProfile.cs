using AutoMapper;
using TennisApplication.Dtos.Tournament;

namespace TennisApplication.Profiles.Tournament
{
    public class TournamentsProfile : Profile
    {
        public TournamentsProfile()
        {
            //source -> target
            CreateMap<Models.Tournament, TournamentReadDto>();
            CreateMap<TournamentCreateDto, Models.Tournament>();
            CreateMap<Models.Tournament, TournamentCreateDto>();

        }
    }
}