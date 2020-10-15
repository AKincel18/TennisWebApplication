using System;

namespace TennisApplication.Dtos
{
    public class TournamentReadDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Place { get; set; }
        
        public DateTime Date { get; set; }
        
        public int PlayersNumber { get; set; }
    }
}