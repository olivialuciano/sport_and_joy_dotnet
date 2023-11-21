using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sport_and_joy_back_dotnet.Entities
{
    public class Field
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } 

        public string? Location { get; set; }
        public string? Image { get; set; }

        public string? Description { get; set; }
        public bool? LockerRoom { get; set; }
        public bool? Bar { get; set; }
        public Esport Sport { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }

        public ICollection<Reservation> Reservations { get; set; }


    }
}
