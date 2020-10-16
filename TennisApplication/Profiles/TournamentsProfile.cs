using AutoMapper;
using TennisApplication.Dtos;
using TennisApplication.Models;

namespace TennisApplication.Profiles
{
    public class TournamentsProfile : Profile
    {
        public TournamentsProfile()
        {
            //source -> target
            CreateMap<Tournament, TournamentReadDto>();
            CreateMap<TournamentCreateDto, Tournament>();
            CreateMap<Tournament, TournamentCreateDto>();

        }
    }
}