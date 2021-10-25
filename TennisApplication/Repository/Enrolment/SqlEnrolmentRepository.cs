using System;
using System.Linq;
using TennisApplication.Database;

namespace TennisApplication.Repository.Enrolment
{
    public class SqlEnrolmentRepository : IEnrolmentRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlEnrolmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void SaveEnrolment(Models.Enrolment enrolment)
        {
            if (enrolment == null)
            {
                throw new ArgumentNullException();
            }

            _context.Enrolments.Add(enrolment);
        }

        public Models.Enrolment FindEnrolment(int userId, int tournamentId)
        {
            return _context.Enrolments.FirstOrDefault(e => e.User.Id == userId && e.Tournament.Id == tournamentId);
        }

        public void DeleteEnrolment(Models.Enrolment enrolment)
        {
            if (enrolment == null)
            {
                throw new ArgumentNullException();
            }

            _context.Enrolments.Remove(enrolment);
        }
    }
}