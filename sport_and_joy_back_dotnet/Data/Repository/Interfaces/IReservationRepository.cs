using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;
using System.Text.RegularExpressions;

namespace sport_and_joy_back_dotnet.Data.Repository.Interfaces
{
    public interface IReservationRepository
    {
        //////// GET ////////
        public Reservation GetResById(int Id);
        public List<Reservation> GetAllRes(); //para el admin q ve todo
        public List<Reservation> GetAllResByUser(int userId);
        public List<Reservation> GetAllResOfFieldsOwner(int userId); //para owner


        //////// POST ////////
        public void CreateRes(Reservation reservation);


        //////// DELETE ////////
        public void DeleteRes(int id, int userId);
        public void DeleteResAdmin(int id);


        //////// PUT ////////
        //No hay put porque no es así la lógica del negocio. no se edita una reserva. simplemente se elimina y se crea otra.


        /////// REPORTS //////
        Task<List<Reservation>> ReservationsInMonth(int month);



    }
}