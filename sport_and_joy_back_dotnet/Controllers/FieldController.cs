using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;

namespace sport_and_joy_back_dotnet.Controllers
{
    [Route("api/Field")]
    [ApiController]
    public class FieldController : ControllerBase
    {
        private readonly IFieldRepository _fieldRepository;

        public FieldController(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }


        //////// GET ////////

        [HttpGet("get/myfields")] //las canchas de un usuario owner
        [Authorize(Roles = "OWNER")]
        public IActionResult GetAllByUser()
        {
            try
            {
                int userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                return Ok(_fieldRepository.GetAllFieByUser(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("getall")] //todas las canchas de la bdd
        [Authorize(Roles = "ADMIN,PLAYER")] //perfecto, el al owner no le trae.

        public IActionResult GetAll()
        {
            try
            {
                var fields = _fieldRepository.GetAllFie().ToList();
                return Ok(fields);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{Id}")] //una cancha en específico x id
        [Authorize(Roles = "ADMIN,OWNER,PLAYER")]

        public IActionResult GetOne(int Id)
        {
            try
            {
                var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                var fields = _fieldRepository.GetFieById(Id);
                return Ok(fields);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }



        //////// POST ////////
        
        // este post se comenta porque quedamos que un owner no puede crear una cancha.

        //[HttpPost("create")] //crear nueva cancha asociada a un usuario
        //[Authorize(Roles = "OWNER")]

        //public IActionResult CreateField(FieldForCreationDTO dto)
        //{
        //    try
        //    {
        //        var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

        //        _fieldRepository.CreateFie(dto, userId);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    return Created("Created", dto);
        //}

        [HttpPost("create-admin")] //crear nueva cancha con userId que le pasa el admin
        [Authorize(Roles = "ADMIN")]

        public IActionResult CreateFieldAdmin(FieldForCreationDTO dto, int IdUser) //el admin le pasa como parámetro el id del usuario dueño de la cancha
        {
            try
            {
                _fieldRepository.CreateFieAdmin(dto, IdUser);
                return Created("Created", dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //////// PUT ////////

        [HttpPut("{id}/edit")] //editar una cancha en específico por id de un usuario logeado. 
        [Authorize(Roles = "OWNER")]

        public IActionResult UpdateField(FieldForCreationDTO dto, int id)
        {
            try
            {
                var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
                _fieldRepository.UpdateFie(dto, userId, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}/edit-admin")] //editar una cancha en específico por id de un usuario que el admin pasa.
        [Authorize(Roles = "ADMIN")]

        public IActionResult UpdateFieldAdmin(FieldForCreationDTO dto, int IdUser, int id)
        {
            try
            {
                _fieldRepository.UpdateFieAdmin(dto, IdUser, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //////// DELETE ////////

        // este delete se comenta porque quedamos que un owner no puede eliminar una cancha.


        //[HttpDelete]
        //[Route("{Id}/delete")] //eliminar una cancha en específico de un usuario en particular
        //[Authorize(Roles = "OWNER")]

        //public IActionResult DeleteFieldById(int id)
        //{
        //    try
        //    {
        //        var userId = Int32.Parse(HttpContext.User.Claims.First(e => e.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        //        _fieldRepository.DeleteFie(id, userId);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //    return Ok();
        //}

        [HttpDelete]
        [Route("{Id}/delete-admin")] //eliminar una cancha en específico.
        [Authorize(Roles = "ADMIN")]

        public IActionResult DeleteFieldByIdAdmin(int id)
        {
            try
            {
                _fieldRepository.DeleteFieAdmin(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}