using System;
using System.Collections.Generic;
using System.Linq;
using TennisApplication.Database;

namespace TennisApplication.Repository.Tournament
{
    public class SqlTournamentRepository : ITournamentRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlTournamentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public IEnumerable<Models.Tournament> GetAllTournaments()
        {
            return _context.Tournaments.ToList();
        }

        public Models.Tournament GetTournamentById(int id)
        {
            return _context.Tournaments.FirstOrDefault(t => t.Id == id);
        }

        public void CreateTournament(Models.Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            _context.Tournaments.Add(tournament);
        }

        public void DeleteTournament(Models.Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException();
            }

            _context.Tournaments.Remove(tournament);
        }

        public IEnumerable<Models.Tournament> GetIncomingTournament()
        {
            return _context.Tournaments
                .Where(tournament => tournament.Date > DateTime.Now)
                .ToList();
        }

        /*public void UpdateTournament(Tournament tournament)
        {
            
        }*/
    }
}