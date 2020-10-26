using System;
using TennisApplication.Models;

namespace TennisApplication.Dtos.Match
{
    [Serializable]
    public class MatchDto
    {
        public int Id { get; set; }
        
        public Models.Tournament Tournament { get; set; }
        
        public Models.User Player1 { get; set; }
        
        public Models.User Player2 { get; set; }
        
        public Winner Winner { get; set; }
        
        public string Result { get; set; }
        
        public int Round { get; set; }

        public MatchDto()
        {
        }

        public MatchDto(Models.Tournament tournament, Models.User player1, Models.User player2, int round)
        {
            Tournament = tournament;
            Player1 = player1;
            Player2 = player2;
            Round = round;
        }

        public MatchDto(int id, Models.Tournament tournament, Models.User player1, Models.User player2, 
            Winner winner, string result, int round)
        {
            Id = id;
            Tournament = tournament;
            Player1 = player1;
            Player2 = player2;
            Winner = winner;
            Result = result;
            Round = round;
        }
    }
}