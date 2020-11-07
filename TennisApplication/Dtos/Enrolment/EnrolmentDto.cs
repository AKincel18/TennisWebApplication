using TennisApplication.Dtos.Tournament;
using TennisApplication.Dtos.User;

namespace TennisApplication.Dtos.Enrolment
{
    public class EnrolmentDto
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }

        public EnrolmentDto(int tournamentId, int userId)
        {
            TournamentId = tournamentId;
            UserId = userId;
        }
    }
}