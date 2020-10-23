using TennisApplication.Dtos.Tournament;
using TennisApplication.Dtos.User;

namespace TennisApplication.Dtos.Enrolment
{
    public class EnrolmentWriteDto
    {
        public int TournamentId { get; set; }
        public int UserId { get; set; }

        public EnrolmentWriteDto(int tournamentId, int userId)
        {
            TournamentId = tournamentId;
            UserId = userId;
        }
    }
}