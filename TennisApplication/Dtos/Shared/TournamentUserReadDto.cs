using TennisApplication.Dtos.Tournament;
using TennisApplication.Dtos.User;

namespace TennisApplication.Dtos.Shared
{
    public class TournamentUserReadDto
    {
        private TournamentReadDto TournamentReadDto { get; set; }
        public UserReadDto UserReadDto { get; set; }
    }
}