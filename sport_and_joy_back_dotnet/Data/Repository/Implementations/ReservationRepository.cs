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


        //////// GET //////// además de reserva, traen algunas propiedades que necesitamos de field y user
        
        public List<Reservation> GetAllResByUser(int userId)
        {
            return _context.Reservations
                .Where(r => r.UserId == userId)
                .Select(r => new Reservation
                {
                    Id = r.Id,
                    Date = r.Date,
                    UserId = r.UserId,
                    FieldId = r.FieldId,

                    Field = new Field
                    {
                        Name = r.Field.Name,
                        Location = r.Field.Location,
                        Price = r.Field.Price,
                        Sport = r.Field.Sport
                    },

                    User = new User
                    {
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        Email = r.User.Email
                    }
                })
                .ToList();
        }

        public List<Reservation> GetAllRes()
        {
            return _context.Reservations
                .Select(r => new Reservation
                {
                    Id = r.Id,
                    Date = r.Date,
                    UserId = r.UserId,
                    FieldId = r.FieldId,

                    Field = new Field
                    {
                        Name = r.Field.Name,
                        Location = r.Field.Location,
                        Price = r.Field.Price,
                        Sport = r.Field.Sport
                    },

                    User = new User
                    {
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        Email = r.User.Email
                    }
                })
                .ToList();
        }

        public Reservation GetResById(int id) //solo está en verde porque puede ser que no haya en la db una reserva de id x. entonces puede retornar null.
        {
            return _context.Reservations
                .Where(r => r.Id == id)
                .Select(r => new Reservation
                {
                    Id = r.Id,
                    Date = r.Date,
                    UserId = r.UserId,
                    FieldId = r.FieldId,

                    Field = new Field
                    {
                        Name = r.Field.Name,
                        Location = r.Field.Location,
                        Price = r.Field.Price,
                        Sport = r.Field.Sport
                    },

                    User = new User
                    {
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        Email = r.User.Email
                    }
                })
                .FirstOrDefault();
        }

        public List<Reservation> GetAllResOfFieldsOwner(int userId)
        {
            return _context.Fields
                .Where(field => field.UserId == userId)
                .SelectMany(field => field.Reservations)
                .Select(r => new Reservation
                {
                    Id = r.Id,
                    Date = r.Date,
                    UserId = r.UserId,
                    FieldId = r.FieldId,

                    Field = new Field
                    {
                        Name = r.Field.Name,
                        Location = r.Field.Location,
                        Price = r.Field.Price,
                        Sport = r.Field.Sport
                    },

                    User = new User
                    {
                        FirstName = r.User.FirstName,
                        LastName = r.User.LastName,
                        Email = r.User.Email
                    }
                })
                .ToList();
        }





        //////// POST ////////
        public void CreateRes(Reservation reservation)
        {
            bool reservationExists = _context.Reservations
                .Any(r => r.FieldId == reservation.FieldId && r.Date.Date == reservation.Date.Date);

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
        public async Task<List<ReportReservationsDTO>> ReservationsInMonth(int month, int year)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Field)
                .Where(r => r.Date.Month == month && r.Date.Year == year)
                .Select(r => new ReportReservationsDTO
                {
                    Id = r.Id,
                    Date = r.Date,
                    FieldId = r.FieldId,
                    UserId = r.UserId,
                    UserFirstName = r.User.FirstName,
                    UserLastName = r.User.LastName,
                    UserEmail = r.User.Email,
                    FieldName = r.Field.Name,
                    FieldPrice = (float)r.Field.Price
                })
                .ToListAsync();
        }


    }
}