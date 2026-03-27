using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.DTOs.Manager;
using PadelManager.Application.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManagersController : ControllerBase
    {
        private readonly IManagerService _managerService;
       

        public ManagersController(IManagerService managerService)
        {
            _managerService = managerService;
            
        }

        #region POST-PUT-PATCH

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateManagerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

 
            try
            {
                var result = await _managerService.AddNewManagerAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear el organizador.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateManagerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            
            try
            {
                var success = await _managerService.UpdateManagerAsync(id, dto);

                if (!success)
                    return NotFound(new { message = $"Organizador con ID: {id} no encontrado." });

                return Ok(new { message = "Organizador actualizado con éxito." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al actualizar.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            
            try
            {
                var success = await _managerService.SoftDeleteToggleManagerAsync(id);

                if (!success)
                    return NotFound(new { message = $"No se encontró el organizador con ID: {id}" });

                return Ok(new { Message = "Estado del organizador actualizado con éxito." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                return Conflict(new { message = "Conflicto de integridad en la base de datos.", detail = ex.InnerException?.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado en el servidor.", detail = ex.Message });
            }
        }

        #endregion

        #region GETS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _managerService.GetAllManagersAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var manager = await _managerService.GetManagerByIdAsync(id);
            if (manager == null) return NotFound();
            return Ok(manager);
        }

        [HttpGet("{id:guid}/tournaments")]
        public async Task<IActionResult> GetWithTournaments(Guid id)
        {
            var manager = await _managerService.GetManagerWithTournamentsAsync(id);
            if (manager == null) return NotFound();
            return Ok(manager);
        }

        [HttpGet("search/email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var manager = await _managerService.GetManagerByEmailAsync(email);
            if (manager == null) return NotFound();
            return Ok(manager);
        }

        [HttpGet("search/dni/{dni}")]
        public async Task<IActionResult> GetByDni(string dni)
        {
            var manager = await _managerService.GetManagerByDniAsync(dni);
            if (manager == null) return NotFound();
            return Ok(manager);
        }

        [HttpGet("search/name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _managerService.GetManagersByNameAsync(name);
            return Ok(result);
        }

        #endregion
    }
}