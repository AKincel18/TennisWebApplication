using System;
using System.ComponentModel.DataAnnotations;

namespace TennisApplication.Models
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Place { get; set; }
        
        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }
        
        public int PlayersNumber { get; set; }
    }
}