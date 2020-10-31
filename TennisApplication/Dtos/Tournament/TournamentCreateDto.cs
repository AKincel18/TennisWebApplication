using System;
using System.ComponentModel.DataAnnotations;

namespace TennisApplication.Dtos.Tournament
{
    public class TournamentCreateDto
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Place { get; set; }
        
        //[Required]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        
        public int PlayersNumber { get; set; }
    }
}