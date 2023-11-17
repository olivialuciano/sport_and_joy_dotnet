using System.ComponentModel.DataAnnotations;

namespace sport_and_joy_back_dotnet.Models
{
    public class FieldForCreationDTO
    {
        [MaxLength(200)]
        [Required]
        public string Name { get; set; }

        public string? Location { get; set; }
        public string? Image { get; set; }

        public string? Description { get; set; }
        public bool? LockerRoom { get; set; }
        public bool? Bar { get; set; }
    }
}
