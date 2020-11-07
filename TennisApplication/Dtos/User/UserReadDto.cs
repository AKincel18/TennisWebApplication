using System.ComponentModel.DataAnnotations;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TennisApplication.Models;
using TennisApplication.Others;

namespace TennisApplication.Dtos.User
{
    public class UserReadDto
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        [DataType(DataType.Password)]  
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string EMail { get; set; }
        
        public Role Role { get; set; }
        
        public byte[] Photo { get; set; }
        public UserReadDto()
        {
        }

        public UserReadDto(int id, string firstName, string password, string eMail, Role role)
        {
            Id = id;
            FirstName = firstName;
            Password = password;
            EMail = eMail;
            Role = role;
        }

        public UserReadDto(int id)
        {
            Id = id;
        }

        public void AvatarPhoto()
        {
            Image image;
            if (Photo != null && Photo.Length != 0)
            {
                image = Image.Load(Photo);
                image = MyImage.ConvertToAvatarPhoto(image) as Image<Rgba32>;
            }
            else
            {
                image = Image.Load(Path.Combine(StringConst.DirImg, StringConst.NameImg));
                image = MyImage.ConvertToAvatarPhoto(image);
            }
                    
            MemoryStream stream = new MemoryStream();
            image.SaveAsPng(stream);
            Photo = stream.ToArray();
            
        }
    }
}