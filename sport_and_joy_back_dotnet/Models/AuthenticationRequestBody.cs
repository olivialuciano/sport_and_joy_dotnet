using System.ComponentModel.DataAnnotations;

namespace sport_and_joy_back_dotnet.Models
{
    public class AuthenticationRequestBody
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
