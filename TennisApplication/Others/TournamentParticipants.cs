using System.Collections.Generic;
using TennisApplication.Dtos.Tournament;
using TennisApplication.Dtos.User;

namespace TennisApplication.Others
{
    public class TournamentParticipants
    {
        public TournamentReadDto TournamentDto { get; set; }
        public List<UserReadDto> Users { get; set; }
        public bool IsRegistered { get; set; }
        public TournamentParticipants(TournamentReadDto tournament, List<UserReadDto> users, bool isRegistered)
        {
            TournamentDto = tournament;
            Users = users;
            IsRegistered = isRegistered;
        }
    }
}