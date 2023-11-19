using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;

namespace sport_and_joy_back_dotnet.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
        public User ValidateUser(AuthenticationRequestBody authRequestBody);
        public User GetById(int userId);
        public List<User> GetAll(); //para el admin q ve todo
        User GetUser(int id);
        public void UpdateUserData(User user);
        List<User> GetListUser();

        User AddUser(User user);
        void DeleteUser(User user);
    }
}
