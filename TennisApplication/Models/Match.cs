using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisApplication.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("TournamentId")]
        public Tournament Tournament { get; set; }
        
        [ForeignKey("Player1Id")]
        public User Player1 { get; set; }
        
        [ForeignKey("Player2Id")]
        public User Player2 { get; set; }
        
        public Winner Winner { get; set; }
        
        public string Result { get; set; }
        
        public int Round { get; set; }
        
    }
}