using sport_and_joy_back_dotnet.Entities;
using System.ComponentModel.DataAnnotations;

namespace sport_and_joy_back_dotnet.Model
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Image { get; set; }

        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public Erole Role { get; set; }

    }
}
