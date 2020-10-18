using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisApplication.Models
{
    public enum Role
    {
        [NotMapped]
        [Display(Name =  "Tournament director")]
        TournamentDirector, 
        
        [NotMapped]
        [Display(Name =  "Player")]
        Player
    }
    
}