using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Manager;
using PadelManager.Application.Interfaces.Services;

namespace PadelManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class ManagersController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagersController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        /// <summary>
        /// Obtiene el perfil completo de un Manager por su ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Organizador")]
        public async Task<IActionResult> GetProfile(Guid id)
        {
            var profile = await _managerService.GetManagerProfileAsync(id);

            if (profile == null)
            {
                return NotFound(new { message = "Perfil de manager no encontrado." });
            }

            return Ok(profile);
        }

        /// <summary>
        /// Lista todos los managers registrados en el sistema.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetAll()
        {
            var managers = await _managerService.GetAllManagersAsync();
            return Ok(managers);
        }

        /// <summary>
        /// Actualiza los datos del perfil del manager y su información de usuario vinculada.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Organizador")]
        public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateManagerDto dto)
        {
            if (dto == null) return BadRequest();

            var result = await _managerService.UpdateManagerProfileAsync(id, dto);

            if (!result)
            {
                return NotFound(new { message = "No se pudo actualizar el perfil. Manager no encontrado." });
            }

            return Ok(new { message = "Perfil actualizado correctamente." });
        }
    }
}