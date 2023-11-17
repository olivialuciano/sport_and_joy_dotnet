using AutoMapper;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using sport_and_joy_back_dotnet.Entities;

namespace sport_and_joy_back_dotnet.Data.Repository.Implementations
{
    public class FieldRepository : IFieldRepository
    {
        private readonly SportContext _context;
        private readonly IMapper _mapper;

        public FieldRepository(SportContext context, IMapper autoMapper)
        {
            _context = context;
            _mapper = autoMapper;
        }
        public void CreateFie(ContactForCreationDTO dto, int userId)
        {
            Contact contactoACargar = new Contact()
            {
                UserId = userId,
                CelularNumber = dto.CelularNumber,
                Description = dto.Description,
                Name = dto.Name,
                TelephoneNumber = dto.TelephoneNumber,
            };
            _context.Contacts.Add(contactoACargar);
            _context.SaveChanges();
        }

        public void DeleteFie(int id, int userId)
        {
            _context.Contacts.Remove(_context.Contacts.Single(c => c.Id == id && c.UserId == userId));
            _context.SaveChanges();
        }

        public List<Field> GetAllFieByUser(int userId)
        {
            return _context.Contacts.Where(c => c.UserId == userId).ToList();
        }

        public Field GetFieById(int contactId)
        {
            return _context.Contacts.FirstOrDefault(c => c.Id == contactId);
        }

        public void UpdateFie(ContactForCreationDTO dto, int userId, int id)
        {
            var contactoAModificar = _context.Contacts.FirstOrDefault(x => x.UserId == userId && x.Id == id);

            if (contactoAModificar != null)
            {
                contactoAModificar.UserId = userId;
                contactoAModificar.Id = id;
                contactoAModificar.CelularNumber = dto.CelularNumber;
                contactoAModificar.Description = dto.Description;
                contactoAModificar.Name = dto.Name;
                contactoAModificar.TelephoneNumber = dto.TelephoneNumber;
                //contactoAModificar.Groups = dto.Groups;

                _context.SaveChanges();
            }
        }
    }
}
