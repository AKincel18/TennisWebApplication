﻿using System;
using System.Collections.Generic;
using System.Linq;
using TennisApplication.Database;
using TennisApplication.Models;

namespace TennisApplication.Repository
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

        public IEnumerable<Tournament> GetAllTournaments()
        {
            return _context.Tournaments.ToList();
        }

        public Tournament GetTournamentById(int id)
        {
            return _context.Tournaments.FirstOrDefault(t => t.Id == id);
        }

        public void CreateTournament(Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            _context.Tournaments.Add(tournament);
        }

        public void DeleteTournament(Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException();
            }

            _context.Tournaments.Remove(tournament);
        }

        /*public void UpdateTournament(Tournament tournament)
        {
            
        }*/
    }
}