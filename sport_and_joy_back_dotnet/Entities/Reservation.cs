using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace sport_and_joy_back_dotnet.Entities
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonProperty("date")]
        //[JsonFormat(pattern: "yyyy-MM-dd")]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }

        [ForeignKey("FieldId")]
        public Field Field { get; set; }
        public int FieldId { get; set; }


    }
}
