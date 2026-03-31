using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PadelManager.Application.DTOs.User;
using PadelManager.Application.Interfaces.Services;

namespace PadelManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protegido por defecto
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        #region CONSULTAS (READ)

        [HttpGet]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user != null ? Ok(user) : NotFound(new { message = "Usuario no encontrado." });
        }

        [HttpGet("search/name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var users = await _userService.GetUsersByNameAsync(name);
            return Ok(users);
        }

        [HttpGet("search/lastname/{lastName}")]
        public async Task<IActionResult> GetByLastName(string lastName)
        {
            var users = await _userService.GetUsersByLastNameAsync(lastName);
            return Ok(users);
        }

        [HttpGet("search/email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet("role/{roleName}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByRole(string roleName)
        {
            var users = await _userService.GetUsersByRoleNameAsync(roleName);
            return Ok(users);
        }

        #endregion

        #region MANTENIMIENTO (CRUD)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newUser = await _userService.AddNewUserAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al crear el usuario.",
                    detail = ex.Message
                });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador, Tanteador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _userService.UpdateUserBasicInfoAsync(id, dto);
                if (!success)
                    return NotFound(new { message = $"Usuario con ID: {id} no encontrado." });

                return Ok(new { message = "Usuario actualizado con éxito." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado al actualizar el usuario.",
                    detail = ex.Message
                });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _userService.SoftDeleteUserAsync(id);
                if (!success)
                    return NotFound(new { message = $"No se encontró el usuario con ID: {id}" });

                return Ok(new { message = "Estado del usuario actualizado con éxito." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new
                {
                    message = "Conflicto de integridad en la base de datos.",
                    detail = ex.InnerException?.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado en el servidor.",
                    detail = ex.Message
                });
            }
        }

        #endregion
    }
}