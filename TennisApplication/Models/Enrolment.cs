using System.ComponentModel.DataAnnotations;

namespace TennisApplication.Models
{
    public class Enrolment
    {
        [Key]
        public int TournamentId { get; set; }
        
        [Key]
        public int UserId { get; set; }
        
    }
}