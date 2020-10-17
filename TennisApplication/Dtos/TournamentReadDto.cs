using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TennisApplication.Dtos
{
    public class TournamentReadDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Place { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        
        [Display(Name = "Number of player")]
        public int PlayersNumber { get; set; }
    }
}