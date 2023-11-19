using sport_and_joy_back_dotnet.Entities;
using System.ComponentModel.DataAnnotations;

namespace sport_and_joy_back_dotnet.Models
{
    public class UserForCreationDTO
    {
        [MaxLength(200)]
        [Required]
        public string Image { get; set; } = "https://www.clipartkey.com/mpngs/m/152-1520367_user-profile-default-image-png-clipart-png-download.png";

        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public Erole Role { get; set; }
    }
}
