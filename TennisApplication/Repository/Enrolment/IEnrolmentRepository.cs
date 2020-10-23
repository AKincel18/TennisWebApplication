namespace TennisApplication.Repository.Enrolment
{
    public interface IEnrolmentRepository
    {
        bool SaveChanges();
        void SaveEnrolment(Models.Enrolment enrolment);
    }
}