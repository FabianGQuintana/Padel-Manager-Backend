using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Registration;
using PadelManager.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        #region POST-PUT-PATCH (Escritura)

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> Post([FromBody] CreateRegistrationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _registrationService.AddNewRegistrationAsync(dto);
                //  Usamos GetById para cumplir con el estándar REST
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                // Captura el error de "pareja ya inscripta" que definimos en el Service
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al procesar la inscripción.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateRegistrationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _registrationService.UpdateRegistrationAsync(id, dto);
                if (!success) return NotFound(new { message = "Inscripción no encontrada." });

                return Ok(new { message = "Inscripción actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la inscripción.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _registrationService.SoftDeleteToggleRegistrationAsync(id);
                if (!success) return NotFound(new { message = "Inscripción no encontrada." });

                return Ok(new { message = "Estado de inscripción actualizado correctamente." });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                return Conflict(new { message = "No se puede modificar la inscripción debido a dependencias en la base de datos.", detail = ex.InnerException?.Message });
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
            var result = await _registrationService.GetAllRegistrationsAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _registrationService.GetRegistrationByIdAsync(id);
            if (result == null) return NotFound(new { message = "Inscripción no encontrada." });
            return Ok(result);
        }

        [HttpGet("tournament/{tournamentId:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByTournament(Guid tournamentId)
        {
            var result = await _registrationService.GetRegistrationsByTournamentIdAsync(tournamentId);
            return Ok(result);
        }

        [HttpGet("category/{categoryId:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var result = await _registrationService.GetRegistrationsByCategoryIdAsync(categoryId);
            return Ok(result);
        }

        [HttpGet("couple/{coupleId:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByCouple(Guid coupleId)
        {
            var result = await _registrationService.GetRegistrationsByCoupleIdAsync(coupleId);
            return Ok(result);
        }

        [HttpGet("date/{date}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByDate(string date)
        {
            // El servicio espera DateOnly, intentamos parsear lo que viene por URL
            if (DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                var result = await _registrationService.GetRegistrationsByDateAsync(parsedDate);
                return Ok(result);
            }
            return BadRequest(new { message = "Formato de fecha inválido. Use YYYY-MM-DD." });
        }

        #endregion
    }
}