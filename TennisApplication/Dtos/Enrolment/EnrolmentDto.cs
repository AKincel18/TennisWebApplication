namespace TennisApplication.Dtos.Enrolment
{
    public class EnrolmentDto
    {
        private int TournamentId { get; set; }
        private int UserId { get; set; }

        public EnrolmentDto(int tournamentId, int userId)
        {
            TournamentId = tournamentId;
            UserId = userId;
        }
    }
}