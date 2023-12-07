namespace sport_and_joy_back_dotnet.Models
{
    public class ReportReservationsDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int FieldId { get; set; }
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string FieldName { get; set; }
        public float FieldPrice { get; set; }
    }
}
