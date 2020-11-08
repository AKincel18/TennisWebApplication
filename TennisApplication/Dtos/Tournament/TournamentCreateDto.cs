using System;
using System.ComponentModel.DataAnnotations;
using TennisApplication.Others;

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
        
        [Required(ErrorMessage = "Draw size field is required")]
        [RegularExpression("2|4|8|16|32|64|128", ErrorMessage = "Draw size must be one of the number: 2, 4, 8, 16, 32, 64, 128")]
        public int DrawSize { get; set; }
    }
}