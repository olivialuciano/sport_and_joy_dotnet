using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;
using System.Text.RegularExpressions;

namespace sport_and_joy_back_dotnet.Data.Repository.Interfaces
{
    public interface IReservationRepository
    {
        public void CreateRes(Reservation reservation);
        public ReservationDTO GetResById(int resId);
        public List<Reservation> GetAllRes(int userId); //para el admin q ve todo
        public List<Reservation> GetAllResByUser(int userId);
        public void DeleteRes(int id, int userId);

        //falta hacer un edit put.
        // y lo de check availability para ver si ya esta alquilada la cancha.
    }
}
