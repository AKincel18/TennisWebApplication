using System.Collections.Generic;

namespace TennisApplication.Repository.Match
{
    public interface IMatchRepository
    {
        bool SaveChanges();
        void SaveMatch(Models.Match match);
        Models.Match GetMatchById(int id);
        List<Models.Match> GetMatchesByTournamentId(int id);
        List<Models.Match> GetMatchesByUserId(int id);
        bool IsAnyMatchInTheTournament(int id);
        int GetTournamentRound(int tournamentId);
    }
}