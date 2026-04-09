using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Court;
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
    public class CourtsController : ControllerBase
    {
        private readonly ICourtService _courtService;

        public CourtsController(ICourtService courtService)
        {
            _courtService = courtService;
        }

        #region POST-PUT-PATCH (Escritura)

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Post([FromBody] CreateCourtDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _courtService.AddNewCourtAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar la cancha.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCourtDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _courtService.UpdateCourtAsync(id, dto);
                if (!success) return NotFound(new { message = "Cancha no encontrada." });

                return Ok(new { message = "Cancha actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la cancha.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _courtService.SoftDeleteToggleCourtAsync(id);
                if (!success) return NotFound(new { message = "Cancha no encontrada." });

                return Ok(new { message = "Estado de la cancha actualizado correctamente." });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                return Conflict(new { message = "Conflicto de base de datos al modificar la cancha.", detail = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado.", detail = ex.Message });
            }
        }

        #endregion

        #region GETS (Lectura y Filtros)

        [HttpGet]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courtService.GetAllCourtsAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _courtService.GetCourtByIdAsync(id);
            if (result == null) return NotFound(new { message = "Cancha no encontrada." });
            return Ok(result);
        }

        [HttpGet("venue/{venueId:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetByVenue(Guid venueId)
        {
            var result = await _courtService.GetCourtsByVenueIdAsync(venueId);
            return Ok(result);
        }

        [HttpGet("search/surface/{surfaceType}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetBySurface(string surfaceType)
        {
            var result = await _courtService.GetCourtsBySurfaceTypeAsync(surfaceType);
            return Ok(result);
        }

        [HttpGet("availability/{availability}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetByAvailability(CourtAvailabilityType availability)
        {
            var result = await _courtService.GetCourtsByAvailabilityAsync(availability);
            return Ok(result);
        }

        #endregion
    }
}