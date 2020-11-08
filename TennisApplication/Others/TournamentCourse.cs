using System;
using System.Collections.Generic;
using System.Linq;
using TennisApplication.Dtos.Match;
using TennisApplication.Dtos.Tournament;
using TennisApplication.Dtos.User;
using TennisApplication.Models;

namespace TennisApplication.Others
{
    [Serializable]
    public class TournamentCourse
    {
        public List<MatchDto> Matches { get; set; }
        
        public List<UserReadDto> UsersDto { get; set; } 
        public int CurrentRound { get; set; }
        public TournamentReadDto TournamentDto { get; set; }
        public bool IsFinished { get; set; }
        public List<string> RoundsName { get; set; }
        public int RoundsNumber { get; set; }
        private UserReadDto _byeUser;

        public TournamentCourse()
        {
        }

        public TournamentCourse(TournamentReadDto tournamentDto)
        {
            TournamentDto = tournamentDto;
            RoundsNumber = (int)Math.Ceiling(Math.Log2(TournamentDto.DrawSize));
            InitNameOfRound();
        }

        public TournamentCourse(List<UserReadDto> users, TournamentReadDto tournamentDto, UserReadDto byeUser)
        {
            UsersDto = users;
            TournamentDto = tournamentDto;
            _byeUser = byeUser;
            RoundsNumber = (int)Math.Ceiling(Math.Log2(TournamentDto.DrawSize));
            InitNameOfRound();
            DrawFirstRound();
        }

        //todo drawing
        private void DrawFirstRound()
        {
            CurrentRound = 1;
            int sizeDraw = TournamentDto.DrawSize;
            List<UserReadDto> draw = new List<UserReadDto>();
            
            for (int i = 0; i < sizeDraw; i++)
            {
                draw.Add(new UserReadDto(-1));
            }
            
            for (int i = UsersDto.Count; i < sizeDraw; i++)
            {
                UsersDto.Add(new UserReadDto(-1));
            }
            
            List<MatchDto> matchesTmp = new List<MatchDto>();
            for (int i = 0; i < sizeDraw / 2; i++)
            {
                matchesTmp.Add(new MatchDto(TournamentDto, UsersDto[i], UsersDto[UsersDto.Count - 1 - i], CurrentRound));
            }
            

            var upperHalf = 0;
            var bottomHalf = sizeDraw - 1;
            for (int i = 0; i < sizeDraw / 2; i++)
            {
                MatchDto match = matchesTmp[i];
                if (i % 2 == 0)
                {
                    draw[upperHalf] = match.Player1;
                    draw[upperHalf + 1] = match.Player2;
                    upperHalf+=2;
                }
                else
                {
                    draw[bottomHalf] = match.Player1;
                    draw[bottomHalf - 1] = match.Player2;
                    bottomHalf-=2;
                }

            }
            

            Matches = new List<MatchDto>();
            for (int i = 0; i < draw.Count; i += 2)
            {
                UserReadDto p1 = draw[i].Id == - 1 ? _byeUser : draw[i];
                UserReadDto p2 = draw[i + 1].Id == - 1 ? _byeUser : draw[i + 1];
                Matches.Add(new MatchDto(TournamentDto, p1, p2, CurrentRound));
                
            }
            
            
        } 
        public void UpdateIds(int pos, int id)
        {
            Matches[pos].Id = id;
        }

        /*public void UpdateMatches(int id, TournamentReadDto tournament, UserReadDto player1, UserReadDto player2,
            int round, Winner winner, string result)
        {
            Matches.Add(new MatchDto(id, tournament, player1, player2, winner, result, round));
        }
        
        public void UpdateMatches(MatchDto matchDto)
        {
            Matches.Add(matchDto);
        }*/

        public void UpdateOthers(TournamentReadDto tournament, int round)
        {
            Matches.RemoveAll(m => m.Id == 0);
            TournamentDto = tournament;
            CurrentRound = round;
            RoundsNumber = (int)Math.Ceiling(Math.Log2(TournamentDto.DrawSize));
            InitNameOfRound();
            if (CurrentRound == RoundsNumber) //tournament finished
            {
                IsFinished = true;
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
                UserReadDto winner1 = matchesPrevRound[i].Winner == Winner.One
                    ? matchesPrevRound[i].Player1
                    : matchesPrevRound[i].Player2;
                
                UserReadDto winner2 = matchesPrevRound[i + 1].Winner == Winner.One
                    ? matchesPrevRound[i + 1].Player1
                    : matchesPrevRound[i + 1].Player2;
                
                Matches.Add(new MatchDto(TournamentDto, winner1, winner2, CurrentRound));
            }
        }

        public void SetTournament(TournamentReadDto tournament, List<MatchDto> matchesInRound)
        {
            foreach (var matchDto in matchesInRound)
            {
                matchDto.TournamentDto = tournament;
            }
        }

        public void UpdateIds(MatchDto matchDto, int matchId)
        {
            int index = Matches.IndexOf(matchDto);
            Matches[index].Id = matchId;
        }

        public List<MatchDto> GetMatchesInCurrentRound()
        {
             return Matches
                .Where(m => m.Round == CurrentRound)
                .ToList();
        }

        private void InitNameOfRound()
        {
            RoundsName = new List<string>();
            for (int i = 1;  i <= RoundsNumber; i++)
            {
                if (i == RoundsNumber)
                {
                    RoundsName.Add("Final");
                } else if (i == RoundsNumber - 1)
                {
                    RoundsName.Add("Semi-final");
                } else if (i == RoundsNumber - 2)
                {
                    RoundsName.Add("Quarter-final");
                }
                else
                {
                    RoundsName.Add("Round " + i);
                }
            }
        }

        public void updateOngoing()
        {
            RoundsNumber = (int)Math.Ceiling(Math.Log2(TournamentDto.DrawSize));
            InitNameOfRound();
            CurrentRound = Matches.Max(m => m.Round);
        }


    }
}