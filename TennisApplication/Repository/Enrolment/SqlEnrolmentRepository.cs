using System;
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
    }
}