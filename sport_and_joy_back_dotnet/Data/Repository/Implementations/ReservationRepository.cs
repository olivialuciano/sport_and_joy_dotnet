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


        //////// GET ////////
        public List<Reservation> GetAllResByUser(int userId)
        {
            return _context.Reservations.Where(r => r.UserId == userId).ToList();
        }

        public List<Reservation> GetAllRes()
        {
            return _context.Reservations.ToList();
        }

        public Reservation GetResById(int id)
        {
            return _context.Reservations.FirstOrDefault(r => r.Id == id);
        }

        public List<Reservation> GetAllResOfFieldsOwner(int userId) //para owner
        {
            var reservations = _context.Fields
                .Where(field => field.UserId == userId) //canchas del usuario owner
                .SelectMany(field => field.Reservations) //todas las reservas asociadas a esas canchas
                .ToList();

            return reservations;
        }




        //////// POST ////////
        public void CreateRes(Reservation reservation)
        {
            bool reservationExists = _context.Reservations // Verifica si ya existe una reserva para la misma cancha y fecha
                .Any(r => r.Field == reservation.Field && r.Date == reservation.Date);

            if (reservationExists)
            {
                throw new InvalidOperationException("Ya existe una reserva ese dia para esa cancha.");
            }
            else
            {
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
            }

        }

        public void CreateResAdmin(Reservation reservation)
        {
            bool reservationExists = _context.Reservations // Verifica si ya existe una reserva para la misma cancha y fecha
                .Any(r => r.Field == reservation.Field && r.Date == reservation.Date);

            if (reservationExists)
            {
                throw new InvalidOperationException("Ya existe una reserva ese dia para esa cancha.");
            }
            else
            {
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
            }

        }



        //////// DELETE ////////
        public void DeleteRes(int id, int userId)
        {
            _context.Reservations.Remove(_context.Reservations.Single(r => r.Id == id && r.UserId == userId));
            _context.SaveChanges();
        }

        public void DeleteResAdmin(int id)
        {
            _context.Reservations.Remove(_context.Reservations.Single(r => r.Id == id));
            _context.SaveChanges();
        }



        //////// PUT ////////
        //No hay put porque no es así la lógica del negocio. no se edita una reserva. simplemente se elimina y se crea otra.



        /////// REPORTS //////
        public async Task<List<Reservation>> ReservationsInMonth(int month)
        {
            return await _context.Reservations
                .Where(r => r.Date.Month == month)
                .ToListAsync();
        }

    }
}