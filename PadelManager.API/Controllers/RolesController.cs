using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Role;
using PadelManager.Application.Interfaces.Services;

namespace PadelManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] //  Por seguridad, solo el Admin gestiona roles
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [AllowAnonymous] // Permitimos que cualquiera los vea (ej: para el combo del Registro)
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener la lista de roles.",
                    detail = ex.Message
                });
            }
        }

        // Si en el futuro querés crear roles desde la API:
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRoleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // Aquí llamarías a un método Add en el servicio si decidís implementarlo
                return Ok(new { message = "Funcionalidad de creación de roles en desarrollo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}