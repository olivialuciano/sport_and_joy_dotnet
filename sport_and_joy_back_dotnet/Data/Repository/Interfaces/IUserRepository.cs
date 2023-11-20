using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;

namespace sport_and_joy_back_dotnet.Data.Repository.Interfaces
{
    public interface IUserRepository
    {

        //////// POST ////////
        /// (autenticar)
        public User ValidateUser(AuthenticationRequestBody authRequestBody);
        /// (registrar)
        User AddUser(User user);

        //////// GET ////////
        public User GetById(int userId);
        public List<User> GetAll(); //para el admin q ve todo
        User GetUser(int id);
        List<User> GetListUser(); //lista de ususarios que usa para registarr user nuevo (que no este en la lista (db))

        //////// PUT ////////
        public void UpdateUserData(User user);

        //////// DELETE ////////
        void DeleteUser(User user);


        /////// REPORTS //////
        Task<List<User>> PlayersWithReservations();
        Task<List<User>> OwnersWithFields();


    }
}
