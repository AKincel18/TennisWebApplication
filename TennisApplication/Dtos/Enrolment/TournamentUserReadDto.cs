using System.Collections.Generic;

namespace TennisApplication.Dtos.Enrolment
{
    public class TournamentUserReadDto
    {
        public Models.Tournament Tournament { get; set; }
        public List<Models.User> Users { get; set; }
        public bool IsRegistered { get; set; }

        public TournamentUserReadDto(Models.Tournament tournament, List<Models.User> users, bool isRegistered)
        {
            Tournament = tournament;
            Users = users;
            IsRegistered = isRegistered;
        }
    }
}