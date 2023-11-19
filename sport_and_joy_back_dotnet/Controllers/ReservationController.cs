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
   // [Authorize]
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

        [HttpPost("create")]
        public IActionResult CreateReservation([FromBody] ReservationForCreationDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            int userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            // Mapear el objeto GroupDTO a un objeto Group
            var reservation = new Reservation
            {
                Date = dto.Date,
                UserId = userId,
                //FieldId = dto.FieldId, ver como luego le pasamos el id de field 

            };

            // Llamar al método en el repositorio para crear el grupo
            _reservationRepository.CreateRes(reservation);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll() //ActionResult -- tipo de devolución que usamos para los endpoints
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value); //es un enum que tiene todos los tipos de claim
            return Ok(_reservationRepository.GetAllResByUser(userId));
        }

        [HttpGet("{Id}")]
        public IActionResult GetOne(int Id)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var reservation = _reservationRepository.GetResById(Id);
            return Ok(reservation);
        }

        //FALTA HACER UN PUT

        [HttpDelete("{Id}/delete")]
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
    }



    
}
