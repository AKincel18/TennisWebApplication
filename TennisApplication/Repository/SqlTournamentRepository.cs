using System.Collections.Generic;
using System.Linq;
using TennisApplication.Database;
using TennisApplication.Models;

namespace TennisApplication.Repository
{
    public class SqlTournamentRepository : ITournamentRepository
    {
        private readonly TournamentContext _context;

        public SqlTournamentRepository(TournamentContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Tournament> GetAllTournaments()
        {
            return _context.Tournaments.ToList();
        }

        public Tournament GetTournamentById(int id)
        {
            return _context.Tournaments.FirstOrDefault(t => t.Id == id);
        }
    }
}