using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisApplication.Models
{
    public class Enrolment
    {
        [Key]
        [Column("TournamentId")]
        public Tournament Tournament { get; set; }
        
        [Key]
        [Column("UserId")]
        public User User { get; set; }

        [NotMapped] 
        public int TournamentId { get; set; }
        
        [NotMapped] 
        public int UserId { get; set; }

    }
}