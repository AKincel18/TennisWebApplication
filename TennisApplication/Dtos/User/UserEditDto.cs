using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TennisApplication.Models;

namespace TennisApplication.Dtos.User
{
    public class UserEditDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter valid Email")]
        public string EMail { get; set; }

        [DataType(DataType.Password)] public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password dose not match.")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        public Role Role { get; set; }
        public byte[] Photo { get; set; }
    }
}