using System.Collections.Generic;

namespace TennisApplication.Repository.Tournament
{
    public interface ITournamentRepository
    {
        bool SaveChanges();
        IEnumerable<Models.Tournament> GetAllTournaments();
        Models.Tournament GetTournamentById(int id);
        void CreateTournament(Models.Tournament tournament);
        void DeleteTournament(Models.Tournament tournament);
        //void UpdateTournament(Tournament tournament);
        IEnumerable<Models.Tournament> GetIncomingTournament();

        List<Models.Tournament> GetCompletedTournaments();

        IEnumerable<Models.Tournament> GetTournamentByUserId(int id);
        IEnumerable<Models.Tournament> GetOngoingTournaments();
        
        IEnumerable<Models.Tournament> GetNotStartedTournaments();
    }
}