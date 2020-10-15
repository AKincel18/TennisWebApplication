using System;
using System.ComponentModel.DataAnnotations;

namespace TennisApplication.Dtos
{
    public class TournamentCreateDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Place { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        public int PlayersNumber { get; set; }
    }
}