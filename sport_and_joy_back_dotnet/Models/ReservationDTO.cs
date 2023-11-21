using Newtonsoft.Json;
using sport_and_joy_back_dotnet.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace sport_and_joy_back_dotnet.Models
{
    public class ReservationDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public int FieldId { get; set; }

        public int UserId { get; set; }

    }
}