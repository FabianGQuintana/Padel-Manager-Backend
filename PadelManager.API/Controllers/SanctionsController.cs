using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Sanction;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SanctionsController : ControllerBase
    {
        private readonly ISanctionService _sanctionService;

        public SanctionsController(ISanctionService sanctionService)
        {
            _sanctionService = sanctionService;
        }

        #region POST-PUT-PATCH (Escritura)

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Post([FromBody] CreateSanctionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _sanctionService.AddNewSanctionAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al aplicar la sanción.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateSanctionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _sanctionService.UpdateSanctionAsync(id, dto);
                if (!success) return NotFound(new { message = "Sanción no encontrada." });

                return Ok(new { message = "Sanción actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la sanción.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _sanctionService.SoftDeleteToggleSanctionAsync(id);
                if (!success) return NotFound(new { message = "Sanción no encontrada." });

                return Ok(new { message = "Estado de la sanción modificado correctamente." });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                return Conflict(new { message = "Error de integridad al modificar la sanción.", detail = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado.", detail = ex.Message });
            }
        }

        #endregion

        #region GETS (Lectura y Filtros)

        [HttpGet]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sanctionService.GetAllSanctionsAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _sanctionService.GetSanctionByIdAsync(id);
            if (result == null) return NotFound(new { message = "Sanción no encontrada." });
            return Ok(result);
        }

        [HttpGet("player/{playerId:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetByPlayer(Guid playerId)
        {
            var result = await _sanctionService.GetSanctionsByPlayerIdAsync(playerId);
            return Ok(result);
        }

        [HttpGet("severity/{severity}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetBySeverity(StatusSeverity severity)
        {
            var result = await _sanctionService.GetSanctionsBySeverityAsync(severity);
            return Ok(result);
        }

        [HttpGet("active")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _sanctionService.GetActiveSanctionsAsync();
            return Ok(result);
        }

        [HttpGet("blocked/{playerId:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> IsPlayerBlocked(Guid playerId)
        {
            var isBlocked = await _sanctionService.IsPlayerBlockedAsync(playerId);
            return Ok(new { playerId, isBlocked, message = isBlocked ? "Jugador inhabilitado." : "Jugador habilitado." });
        }

        #endregion
    }
}