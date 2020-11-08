namespace TennisApplication.Repository.Enrolment
{
    public interface IEnrolmentRepository
    {
        bool SaveChanges();
        void SaveEnrolment(Models.Enrolment enrolment);
        Models.Enrolment FindEnrolment(int userId, int tournamentId);
        void DeleteEnrolment(Models.Enrolment enrolment);
    }
}