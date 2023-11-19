using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            var reservation = _context.Reservations
                .Include(f => f.FieldId)
                .FirstOrDefault(r => r.Id == resId);

            if (reservation == null)
            {
                return null; 
            }

            var reservationDTO = new ReservationDTO
            {
                Id = reservation.Id,
                Date = reservation.Date,
                Field = new Field
                {
                    Id = reservation.Field.Id,
                    Location = reservation.Field.Location,
                    Description = reservation.Field.Description,
                    LockerRoom = reservation.Field.LockerRoom,
                    Bar = reservation.Field.Bar,
                    Sport = reservation.Field.Sport,
                }
            };

            return reservationDTO;
        }


        public void CreateRes(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }

        public void DeleteRes(int id, int userId)
        {
            _context.Reservations.Remove(_context.Reservations.Single(r => r.Id == id && r.UserId == userId));
            _context.SaveChanges();
        }


        public List<Reservation> GetAllResByUser(int userId)
        {
            return _context.Reservations.Where(r => r.UserId == userId).ToList();
        }
        public List<Reservation> GetAllRes(int userId)
        {
            return _context.Reservations.ToList();
        }
    }
}
