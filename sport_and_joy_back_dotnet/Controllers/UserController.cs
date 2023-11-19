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

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            return Ok(_userRepository.GetAll());
        }

        [HttpGet("{Id}")]
        [Authorize]
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


        [HttpPut("{id}/edit")] //para editar nombre, foto, apellido y email.
        [Authorize]
        public IActionResult EditUserData(int id, UserForModificationDTO dto)
        {
            try
            {
                int userSesionId = Int32.Parse(HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                var user = new User()
                {
                    Id = dto.Id,
                    Image = dto.Image,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                };


                if (id != userSesionId)
                {
                    return Unauthorized();
                }

                if (id != user.Id)
                {
                    return Unauthorized();
                }

                var userItem = _userRepository.GetUser(id);

                if (userItem == null)
                {
                    return NotFound();
                }

                _userRepository.UpdateUserData(user);

                var userModificado = _userRepository.GetUser(id);

                var userModificadoDto = _mapper.Map<UserForCreationDTO>(userModificado);

                return Ok(userModificadoDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}/delete")]
        [Authorize]
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

                if (id != userSesionId)
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

        [HttpPost("authorization")]
        public ActionResult<string> Autenticar(AuthenticationRequestBody authenticationRequestBody)
        {
            //Validamos las credenciales
            //llamar a una función que valide los parámetros que enviamos.
            var user = _userRepository.ValidateUser(authenticationRequestBody);

            if (user is null)
                return Unauthorized(); //st 401

            //Creación el token
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));

            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            //CLAIMS - data-valor ---- LA PARTE DEL PAYLOAD DEL JWT
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString())); //esto lo usamos para traer las cosas que pertenecen a un user en particular.
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
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

        [HttpPost("registration")]//("newuser")
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

                var userItemDto = _mapper.Map<UserForCreationDTO>(userItem);

                return Created("Created", userItemDto); ///*************
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
