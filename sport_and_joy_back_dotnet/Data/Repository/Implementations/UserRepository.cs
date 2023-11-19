using AutoMapper;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;

namespace sport_and_joy_back_dotnet.Data.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private SportContext _context;
        private readonly IMapper _mapper;
        public UserRepository(SportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public User? GetById(int userId)
        {
            return _context.Users.SingleOrDefault(u => u.Id == userId);
        }

        public User? ValidateUser(AuthenticationRequestBody authRequestBody)
        {
            return _context.Users.FirstOrDefault(p => p.Email == authRequestBody.Email && p.Password == authRequestBody.Password);
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }
        public void UpdateUserData(User user)
        {
            var userItem = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            if (userItem != null)
            {
                userItem.FirstName = user.FirstName;
                userItem.Email = user.Email;
                userItem.LastName = user.LastName;

                _context.SaveChanges();
            }
        }
        public User AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        public List<User> GetListUser()
        {
            return _context.Users.ToList();
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }


    }
}
