using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TennisApplication.Dtos
{
    public class TournamentCreateDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Place { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        
        public int PlayersNumber { get; set; }
    }
}