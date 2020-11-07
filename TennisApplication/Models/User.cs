using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisApplication.Models
{
    [Serializable]
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string EMail { get; set; }
        
        [DataType(DataType.Password)]  
        [Required]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]  
        [NotMapped]
        public string ConfirmPassword { get; set; }
        
        [EnumDataType(typeof(Role))]
        public Role Role { get; set; }

        public byte[] Photo { get; set; }

        public User()
        {
            
        }
    }
}