using System.Collections.Generic;
using TennisApplication.Models;

namespace TennisApplication.Repository
{
    public interface ITournamentRepository
    {
        bool SaveChanges();
        IEnumerable<Tournament> GetAllTournaments();
        Tournament GetTournamentById(int id);
        void CreateTournament(Tournament tournament);
        void DeleteTournament(Tournament tournament);
        //void UpdateTournament(Tournament tournament);
    }
}