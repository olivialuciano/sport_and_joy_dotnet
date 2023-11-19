using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using sport_and_joy_back_dotnet.Models;

namespace sport_and_joy_back_dotnet.Controllers
{
    [Route("api/Field")]
    [ApiController]
    [Authorize]
    public class FieldController : ControllerBase
    {
        private readonly IFieldRepository _fieldRepository;

        public FieldController(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            return Ok(_fieldRepository.GetAllFieByUser(userId));
        }

        [HttpGet("{Id}")]
        public IActionResult GetOne(int Id)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var fields = _fieldRepository.GetAllFie(userId).Where(x => x.Id == Id && x.UserId == userId).ToList();
            return Ok(fields);

        }

        [HttpPost("create")]
        public IActionResult CreateField(FieldForCreationDTO dto)
        {
            try
            {
                var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                _fieldRepository.CreateFie(dto, userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Created("Created", dto);
        }

        [HttpPut("{id}/edit")]
        public IActionResult UpdateField(FieldForCreationDTO dto, int id)
        {
            try
            {
                var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                _fieldRepository.UpdateFie(dto, userId, id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("{Id}/delete")]
        public IActionResult DeleteFieldById(int id)
        {
            try
            {
                var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                _fieldRepository.DeleteFie(id, userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }

    }
}
