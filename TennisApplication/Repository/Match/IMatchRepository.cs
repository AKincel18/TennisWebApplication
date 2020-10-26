using System.Collections.Generic;

namespace TennisApplication.Repository.Match
{
    public interface IMatchRepository
    {
        bool SaveChanges();
        void SaveMatch(Models.Match match);
        List<Models.Match> GetNotFinishedMatches();

        Models.Match GetMatchById(int id);
    }
}