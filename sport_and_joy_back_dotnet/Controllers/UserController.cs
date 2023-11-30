using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using sport_and_joy_back_dotnet.Entities;
using sport_and_joy_back_dotnet.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sport_and_joy_back_dotnet.Controllers
{
    [Route("api/User")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UserController(IUserRepository userRepository, IMapper mapper, IConfiguration config)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
        }


        //////// GET ////////

        [HttpGet("getall")] //trae todos los usuarios
        [Authorize(Roles = "ADMIN")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_userRepository.GetAll());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get/{Id}")] //trae un usuario en especifico x id
        [Authorize(Roles = "ADMIN")]
        public IActionResult GetOneById(int Id)
        {
            try
            {
                return Ok(_userRepository.GetById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //////// POST ////////

        [HttpPost("authorization")] //es el login, el usuario inicia sesión.
        public ActionResult<string> Autenticar(AuthenticationRequestBody authenticationRequestBody)
        {
            //Validamos las credenciales
            var user = _userRepository.ValidateUser(authenticationRequestBody);

            if (user is null)
                return Unauthorized();

            //Creación el token
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));

            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            //CLAIMS - data-valor ---- LA PARTE DEL PAYLOAD DEL JWT
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString())); //esto lo usamos para traer las cosas que pertenecen a un user en particular.
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("role", user.Role.ToString())); //agregamos el rol como claim para agarrarlo luegoen le front.

            //ACÁ SE CREA EL TOKEN
            var jwtSecurityToken = new JwtSecurityToken(
              _config["Authentication:Issuer"],
              _config["Authentication:Audience"],
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              credentials);

            var tokenToReturn = new JwtSecurityTokenHandler() //Pasamos el token a string
                .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        [HttpPost("registration")] //el usuario se crea una cuenta
        public IActionResult PostUser(UserForCreationDTO dto)
        {
            try
            {
                var user = new User()
                {
                    Image = dto.Image,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = dto.Password,
                    Role = dto.Role,
                };
                var usersActivos = _userRepository.GetListUser();
                foreach (var userActivo in usersActivos)
                {
                    if (user.Email == userActivo.Email)
                    {
                        return BadRequest("El email ingresado ya es utilizado en una cuenta activa");
                    }
                }
                var userItem = _userRepository.AddUser(user);
                var userItemDtoRta = _mapper.Map<UserForModificationDTO>(userItem);
                return Created("Created", userItemDtoRta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registration-adduser-admin")] 
        [Authorize(Roles = "ADMIN")] //autorizacion solo a admin para que el admin cree ususarios
        // está al pedo porque es igual que el otro? podemos simplemente usar el otro en el front en un componente para el admin?
        public IActionResult PostUserAdmin(UserForCreationDTO dto)
        {
            try
            {
                var user = new User()
                {
                    Image = dto.Image,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = dto.Password,
                    Role = dto.Role,
                };
                var usersActivos = _userRepository.GetListUser();
                foreach (var userActivo in usersActivos)
                {
                    if (user.Email == userActivo.Email)
                    {
                        return BadRequest("El email ingresado ya es utilizado en una cuenta activa");
                    }
                }
                var userItem = _userRepository.AddUser(user);
                var userItemDtoRta = _mapper.Map<UserForModificationDTO>(userItem);
                return Created("Created", userItemDtoRta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //////// PUT ////////

        [HttpPut("{idUserLoggedIn}/edit")] //PERFECTO
        [Authorize(Roles = "ADMIN,PLAYER,OWNER")] // modificar datos del usuario logeado.
        public IActionResult EditUserData(int idUserLoggedIn, UserForModificationDTO dto)
        {
            try
            {
                int userSesionId = Int32.Parse(HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                var user = new User()
                {
                    Id = idUserLoggedIn,
                    Image = dto.Image,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                };
                if (idUserLoggedIn != userSesionId)
                {
                    return Unauthorized();
                }
                if (idUserLoggedIn != user.Id)
                {
                    return Unauthorized();
                }
                var userItem = _userRepository.GetUser(idUserLoggedIn);
                if (userItem == null)
                {
                    return NotFound();
                }
                _userRepository.UpdateUserData(user);
                var userModificado = _userRepository.GetUser(idUserLoggedIn);
                var userModificadoDtoRta = _mapper.Map<UserForModificationDTO>(userModificado);
                return Ok(userModificadoDtoRta);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPut("{idUserToModify}/edit-admin")] //PERFECTO
        [Authorize(Roles = "ADMIN")] //PARA EL ADMIN. básicamente lo mismo pero no corrobora que el id sea igual al id del user logeado.
        public IActionResult EditUserDataAdmin(int idUserToModify, UserForModificationDTO dto)
        {
            try
            {
                var user = new User()
                {
                    Id = idUserToModify,
                    Image = dto.Image,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Role = dto.Role,
                };
                var userItem = _userRepository.GetUser(idUserToModify);
                if (userItem == null)
                {
                    return NotFound();
                }
                _userRepository.UpdateUserData(user);
                var userModificado = _userRepository.GetUser(idUserToModify);
                var userModificadoDtoRta = _mapper.Map<UserForModificationDTO>(userModificado);
                return Ok(userModificadoDtoRta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        //////// DELETE ////////


        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "ADMIN,PLAYER,OWNER")] // eliminación del usuario logeado
        public IActionResult DeleteUser(int id)
        {
            try
            {
                int userSesionId = Int32.Parse(HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                var user = _userRepository.GetUser(id);
                if (user == null)
                {
                    return NotFound();
                }
                if (id != userSesionId) //VER ACÁ TEMA ADMIN agregar que el rol sea diferente de admin o algo así
                {
                    return Unauthorized();
                }
                _userRepository.DeleteUser(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}/delete-admin")]
        [Authorize(Roles = "ADMIN")] // eliminación de usarios (admin)
        public IActionResult DeleteUserAdmin(int id)
        {
            try
            {
                var user = _userRepository.GetUser(id);
                if (user == null)
                {
                    return NotFound();
                }
                _userRepository.DeleteUser(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}