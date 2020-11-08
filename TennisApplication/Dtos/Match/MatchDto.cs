using System;
using TennisApplication.Dtos.Tournament;
using TennisApplication.Dtos.User;
using TennisApplication.Models;

namespace TennisApplication.Dtos.Match
{
    [Serializable]
    public class MatchDto
    {
        public int Id { get; set; }
        
        public TournamentReadDto TournamentDto { get; set; }
        
        public UserReadDto Player1 { get; set; }
        
        public UserReadDto Player2 { get; set; }
        
        public Winner Winner { get; set; }
        
        public string Result { get; set; }
        
        public int Round { get; set; }

        public MatchDto()
        {
        }

        public MatchDto(TournamentReadDto tournamentDto, UserReadDto player1, UserReadDto player2, int round)
        {
            TournamentDto = tournamentDto;
            Player1 = player1;
            Player2 = player2;
            Round = round;
        }

        public MatchDto(TournamentReadDto tournamentDto, UserReadDto player1, UserReadDto player2, int round, Winner winner) 
            : this(tournamentDto, player1, player2, round)
        {
            Winner = winner;
        }

        public MatchDto(int id, TournamentReadDto tournamentDto, UserReadDto player1, UserReadDto player2, 
            Winner winner, string result, int round)
        {
            Id = id;
            TournamentDto = tournamentDto;
            Player1 = player1;
            Player2 = player2;
            Winner = winner;
            Result = result;
            Round = round;
        }
    }
}