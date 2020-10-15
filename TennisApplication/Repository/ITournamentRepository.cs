using System.Collections.Generic;
using TennisApplication.Models;

namespace TennisApplication.Repository
{
    public interface ITournamentRepository
    {
        IEnumerable<Tournament> GetAllTournaments();
        Tournament GetTournamentById(int id);
    }
}