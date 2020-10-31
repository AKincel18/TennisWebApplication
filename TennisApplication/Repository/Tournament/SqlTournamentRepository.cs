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
                .Where(tournament => tournament.Completed == false)
                .ToList();
        }

        public List<Models.Tournament> GetCompletedTournaments()
        {
            return _context.Tournaments
                .Where(t => t.Completed == true)
                .ToList();
        }

        public IEnumerable<Models.Tournament> GetTournamentByUserId(int id)
        {
            List<int> tournamentIds =
                _context.Enrolments
                    .Where(e => e.User.Id == id)
                    .Select(e => e.Tournament.Id)
                    .ToList();

            List<Models.Tournament> tournaments = _context.Tournaments.ToList();

            var query = from tournament in tournaments
                join id1 in tournamentIds on tournament.Id equals id1
                select new {Tournament = tournament};

            return query.Select(t => t.Tournament);


        }

        /*public void UpdateTournament(Tournament tournament)
        {
            
        }*/
    }
}