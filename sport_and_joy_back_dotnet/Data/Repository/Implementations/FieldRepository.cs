using AutoMapper;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;

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
        public void CreateFie(FieldForCreationDTO dto, int userId)
        {
            Field field = new Field()
            {
                UserId = userId,
                Location = dto.Location,
                Description = dto.Description,
                LockerRoom = dto.LockerRoom,
                Bar = dto.Bar,
                Sport = dto.Sport,
            };
            _context.Fields.Add(field);
            _context.SaveChanges();
        }

        public void DeleteFie(int id, int userId)
        {
            _context.Fields.Remove(_context.Fields.Single(f => f.Id == id && f.UserId == userId));
            _context.SaveChanges();
        }

        public List<Field> GetAllFieByUser(int userId)
        {
            return _context.Fields.Where(f => f.UserId == userId).ToList();
        }

        public Field GetFieById(int id)
        {
            return _context.Fields.FirstOrDefault(f => f.Id == id);
        }

        public void UpdateFie(FieldForCreationDTO dto, int userId, int id)
        {
            var field = _context.Fields.FirstOrDefault(f => f.UserId == userId && f.Id == id);

            if (field != null)
            {
                field.UserId = userId;
                field.Id = id;
                field.Location = dto.Location;
                field.Description = dto.Description;
                field.LockerRoom = dto.LockerRoom;
                field.Bar = dto.Bar;
                field.Sport = dto.Sport;

                _context.SaveChanges();
            }
        }

        public List<Field> GetAllFie(int userId)
        {
            return _context.Fields.ToList();
        }

    }
}
