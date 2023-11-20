using sport_and_joy_back_dotnet.Data;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace sport_and_joy_back_dotnet.Controllers
{
    [Route("api/Reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IFieldRepository _fieldRepository;
        private readonly SportContext _context;

        public ReservationController(SportContext context, IReservationRepository reservationRepository, IFieldRepository fieldRepository)
        {
            _context = context;
            _reservationRepository = reservationRepository;
            _fieldRepository = fieldRepository;
        }

        //////// GET ////////

        [HttpGet("myreservations")] //las reservas de un usario player
        [Authorize(Roles = "PLAYER")]

        public IActionResult GetAllByUser() 
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value); //es un enum que tiene todos los tipos de claim
            return Ok(_reservationRepository.GetAllResByUser(userId));
        }


        [HttpGet("getall")] //todas las reservas de la bdd solo admin
        [Authorize(Roles = "ADMIN")]

        public IActionResult GetAll(int Id)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var reservations = _reservationRepository.GetAllRes(userId).Where(x => x.Id == Id && x.UserId == userId).ToList();
            return Ok(reservations);
        }


        [HttpGet("{Id}")] //una reserva en específico x id
        [Authorize(Roles = "ADMIN,PLAYER,OWNER")]
        public IActionResult GetOne(int Id)
        {
            var reservation = _reservationRepository.GetResById(Id);
            return Ok(reservation);
        }



        //////// POST ////////

        [HttpPost("create")] //crea reserva asociada al userid logeado.
        [Authorize(Roles = "PLAYER")]

        public IActionResult CreateReservation([FromBody] ReservationForCreationDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            int userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var reservation = new Reservation
            {
                Date = dto.Date,
                UserId = userId,
                FieldId = dto.FieldId,

            };

            _reservationRepository.CreateRes(reservation);

            return Ok();
        }


        [HttpPost("create-admin")] //crea reserva ASOCIADA AL ID DE UN USUARIO QUE EL ADMIN PROPORCIONE
        [Authorize(Roles = "ADMIN")] 

        public IActionResult CreateReservationAdmin([FromBody] ReservationForCreationDTO dto, int IdUser)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            var reservation = new Reservation
            {
                Date = dto.Date,
                UserId = IdUser, //id proporcionado x el admin
                FieldId = dto.FieldId,

            };

            _reservationRepository.CreateResAdmin(reservation);

            return Ok();
        }



        //////// DELETE ////////
        
        [HttpDelete("{Id}/delete")] //eliminar una reserva en específico por id de usuario.
        [Authorize(Roles = "PLAYER")]

        public IActionResult DeleteReservationById(int id)
        {
            try
            {
                var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                _reservationRepository.DeleteRes(id, userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }


        [HttpDelete("{Id}/delete-admin")] //eliminar una reserva en específico admin proporciona id de usuario.
        [Authorize(Roles = "ADMIN")] 

        public IActionResult DeleteReservationByIdAdmin(int id)
        {
            try
            {
                _reservationRepository.DeleteResAdmin(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }


        //////// PUT ////////
        //No hay put porque no es así la lógica del negocio. no se edita una reserva. simplemente se elimina y se crea otra.


    }




}
