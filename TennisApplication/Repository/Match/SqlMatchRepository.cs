using System;
using System.Collections.Generic;
using System.Linq;
using TennisApplication.Database;

namespace TennisApplication.Repository.Match
{
    public class SqlMatchRepository : IMatchRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlMatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void SaveMatch(Models.Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            _context.Matches.Add(match);
        }

        public List<Models.Match> GetNotFinishedMatches()
        {
            return _context.Matches.Where(m => m.Result == null).ToList();
        }

        public Models.Match GetMatchById(int id)
        {
            return _context.Matches.FirstOrDefault(m => m.Id == id);
        }
    }
}