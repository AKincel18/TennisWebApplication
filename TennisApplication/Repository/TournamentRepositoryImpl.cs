using System;
using System.Collections.Generic;
using TennisApplication.Models;

namespace TennisApplication.Repository
{
    public class TournamentRepositoryImpl : ITournamentRepository
    {
        public IEnumerable<Tournament> GetAllTournaments()
        {
            var tournaments = new List<Tournament>
            {
                new Tournament {Id = 0, Name = "Tournament nr 1", Place = "Katowice", Date = DateTime.Now, PlayersNumber = 16},
                new Tournament {Id = 1, Name = "Tournament nr 2", Place = "Gliwice", Date = DateTime.Now, PlayersNumber = 32},
                new Tournament {Id = 2, Name = "Tournament nr 3", Place = "Chorzów", Date = DateTime.Now, PlayersNumber = 64}
            };
            return tournaments;
        }

        public Tournament GetTournamentById(int id)
        {
            return new Tournament {Id = id, Name = "Tournament test", Place = "Katowice", Date = DateTime.Now, PlayersNumber = 16};
        }
    }
}