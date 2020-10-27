using System;
using System.Collections.Generic;
using System.Linq;
using TennisApplication.Dtos.Match;
using TennisApplication.Models;

namespace TennisApplication.Others
{
    [Serializable]
    public class TournamentCourse
    {
        public List<MatchDto> Matches { get; set; }
        
        public List<User> Users { get; set; } 
        public int CurrentRound { get; set; }
        public Tournament Tournament { get; set; }
        public bool isFinished { get; set; }

        public TournamentCourse()
        {
        }

        public TournamentCourse(List<User> users, Tournament tournament)
        {
            Users = users;
            Tournament = tournament;
        }

        //todo drawing
        public void DrawFirstRound()
        {
            CurrentRound = 1;
            Matches = new List<MatchDto>();
            for (int i = 0; i < Users.Count; i += 2)
            {
                Matches.Add(new MatchDto(Tournament, Users[i], Users[i + 1], CurrentRound));
            }
        }
        
        
        public void UpdateIds(int pos, int id)
        {
            Matches[pos].Id = id;
        }

        public void UpdateMatches(int id, Tournament tournament, User player1, User player2,
            int round, Winner winner, string result)
        {
            Matches.Add(new MatchDto(id, tournament, player1, player2, winner, result, round));
        }

        public void UpdateOthers(Tournament tournament, int round)
        {
            Matches.RemoveAll(m => m.Id == 0);
            Tournament = tournament;
            CurrentRound = round;
            if ((int)Math.Pow(2, CurrentRound) == Tournament.PlayersNumber) //tournament finished
            {
                isFinished = true;
            }
            else
            {
                CreateNextMatches();
            }
        }

        private void CreateNextMatches()
        {
            List<MatchDto> matchesPrevRound = Matches.Where(m => m.Round == CurrentRound).ToList();
            
            CurrentRound += 1;
            for (int i = 0; i < matchesPrevRound.Count; i += 2)
            {
                User winner1 = matchesPrevRound[i].Winner == Winner.One
                    ? matchesPrevRound[i].Player1
                    : matchesPrevRound[i].Player2;
                
                User winner2 = matchesPrevRound[i + 1].Winner == Winner.One
                    ? matchesPrevRound[i + 1].Player1
                    : matchesPrevRound[i + 1].Player2;
                
                Matches.Add(new MatchDto(Tournament, winner1, winner2, CurrentRound));
            }
        }

        public void SetTournament(Tournament tournament, List<MatchDto> matchesInRound)
        {
            foreach (var matchDto in matchesInRound)
            {
                matchDto.Tournament = tournament;
            }
        }

        public void UpdateIds(MatchDto matchDto, int matchId)
        {
            int index = Matches.IndexOf(matchDto);
            Matches[index].Id = matchId;
        }
    }
}