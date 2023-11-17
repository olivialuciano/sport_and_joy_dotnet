using AutoMapper;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;
using System.Text.RegularExpressions;

namespace sport_and_joy_back_dotnet.Data.Repository.Implementations
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly SportContext _context;
        private readonly IMapper _mapper;

        public ReservationRepository(SportContext context, IMapper autoMapper)
        {
            _context = context;
            _mapper = autoMapper;
        }

        public ReservationDTO GetResById(int resId)
        {
            var reservation = _context.Groups
                .Include(g => g.Contacts)
                .FirstOrDefault(g => g.Id == resId);

            if (reservation == null)
            {
                return null; 
            }

            var groupDTO = new ReservationDTO
            {
                Id = group.Id,
                Name = group.Name,
                Contacts = group.Contacts.Select(contact => new ContactDTO
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    CelularNumber = contact.CelularNumber,
                    Description = contact.Description,
                    TelephoneNumber = contact.TelephoneNumber
                }).ToList()
            };

            return groupDTO;
        }

        public void CreateRes(Group group)
        {
            _context.Groups.Add(group);
            _context.SaveChanges();
        }

        public void DeleteRes(int id, int userId)
        {
            _context.Groups.Remove(_context.Groups.Single(c => c.Id == id && c.UserId == userId));
            _context.SaveChanges();
        }


        public List<Reservation> GetAllResByUser(int userId)
        {
            return _context.Groups.Where(g => g.UserId == userId).ToList();
        }
    }
}
