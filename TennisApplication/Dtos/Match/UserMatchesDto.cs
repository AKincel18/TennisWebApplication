using System.Collections.Generic;
using TennisApplication.Dtos.User;

namespace TennisApplication.Dtos.Match
{
    public class UserMatchesDto
    {
        public List<MatchDto> Matches { get; }
        public UserReadDto Player { get; }

        public UserMatchesDto(List<MatchDto> matches, UserReadDto player)
        {
            Matches = matches;
            Player = player;
        }
    }
}