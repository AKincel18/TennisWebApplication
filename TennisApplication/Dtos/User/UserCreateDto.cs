using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TennisApplication.Models;

namespace TennisApplication.Dtos.User
{
    public class UserCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [Display(Name = "E-mail")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter valid Email")]
        public string EMail { get; set; }
        
        [Required(ErrorMessage = "Please provide Password", AllowEmptyStrings = false)]  
        [DataType(DataType.Password)]  
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 6 char long.")]
        public string Password { get; set; }
        
        [Compare("Password", ErrorMessage = "Confirm password does not match.")]  
        [DataType(DataType.Password)]  
        [Display(Name = "Confirm password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        
        public Role Role { get; set; }
        
        public byte[] Photo { get; set; }
    }
}