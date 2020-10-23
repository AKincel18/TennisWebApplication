using System.ComponentModel.DataAnnotations;
using TennisApplication.Models;

namespace TennisApplication.Dtos.User
{
    public class UserReadDto
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        [DataType(DataType.Password)]  
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string EMail { get; set; }
        
        public Role Role { get; set; }
        
    }
}