using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public Models.Match GetMatchById(int id)
        {
            return _context.Matches
                .Include(m => m.Player1) //fetch players
                .Include(m => m.Player2) 
                .FirstOrDefault(m => m.Id == id);
        }

        public List<Models.Match> GetMatchesByTournamentId(int id)
        {
            return _context.Matches
                .Include(m => m.Player1) //fetch players
                .Include(m => m.Player2) 
                .Where(m => m.Tournament.Id == id)
                .ToList();
        }

        public List<Models.Match> GetMatchesByUserId(int id)
        {
            return _context.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Include(m => m.Tournament)
                .Where(m => m.Player1.Id == id || m.Player2.Id == id)
                .ToList();
        }

        public bool IsAnyMatchInTheTournament(int id)
        {
            return _context.Matches.Any(m => m.Tournament.Id == id);
        }

        public int GetTournamentRound(int tournamentId)
        {
            return _context.Matches.Where(m => m.Tournament.Id == tournamentId).Max(m => m.Round);
        }
    }
}