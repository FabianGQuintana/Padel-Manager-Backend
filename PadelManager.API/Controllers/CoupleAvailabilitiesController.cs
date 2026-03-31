using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.CoupleAvailability;
using PadelManager.Application.Interfaces.Services;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoupleAvailabilitiesController : ControllerBase
    {
        private readonly ICoupleAvailabilityService _availabilityService;

        public CoupleAvailabilitiesController(ICoupleAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        #region PUT-PATCH-POST

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> Post([FromBody] CreateCoupleAvailabilityDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newAvailability = await _availabilityService.AddNewAvailabilityAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newAvailability.Id }, newAvailability);
            }
            catch (InvalidOperationException ex)
            {
                // Aquí caerá la lógica de "torneo ya empezado"
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear la disponibilidad.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCoupleAvailabilityDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _availabilityService.UpdateAvailabilityAsync(id, dto);
                if (!success) return NotFound(new { message = $"Disponibilidad con ID: {id} no encontrada." });

                return Ok(new { message = "Disponibilidad actualizada con éxito." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la disponibilidad.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _availabilityService.SoftDeleteToggleAvailabilityAsync(id);
                if (!success) return NotFound(new { message = $"No se encontró la disponibilidad con ID: {id}" });

                return Ok(new { message = "Estado de disponibilidad actualizado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado en el servidor.", detail = ex.Message });
            }
        }

        #endregion

        #region GETS

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _availabilityService.GetAvailabilityByIdAsync(id);
            if (result == null) return NotFound($"No se encontró disponibilidad con ID: {id}");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _availabilityService.GetAllAvailabilitiesAsync();
            return Ok(result);
        }

        [HttpGet("couple/{coupleId:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetByCoupleId(Guid coupleId)
        {
            var result = await _availabilityService.GetAvailabilitiesByCoupleIdAsync(coupleId);
            return Ok(result);
        }

        #endregion
    }
}