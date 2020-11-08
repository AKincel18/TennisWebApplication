using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TennisApplication.Models;

namespace TennisApplication.Dtos.User
{
    public class UserEditDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        
        //[Required(ErrorMessage = "Please provide Password", AllowEmptyStrings = true)]  
        [DataType(DataType.Password)]  
        //[StringLength(50, MinimumLength = 4, ErrorMessage = "Password must be 4 char long.")] 
        public string Password { get; set; }
        
        [Compare("Password", ErrorMessage = "Confirm password dose not match.")]  
        [DataType(DataType.Password)]  
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public Role Role { get; set; }
        public byte[] Photo { get; set; }
        
    }
}