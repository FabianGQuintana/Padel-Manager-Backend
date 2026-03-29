using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Registration;
using PadelManager.Application.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  //Lo ponemos aca para decirle a todos los metodos de la clase que son privados, no pueden acceder
    //Sin tener la autorizacion del token. Si queremos que algunos sean visibles para cualquier usuario ponemos [AllowAnonymous]

    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        #region POST - PUT - PATCH

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRegistrationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newRegistration = await _registrationService.AddNewRegistrationAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newRegistration.Id }, newRegistration);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear la inscripción.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateRegistrationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _registrationService.UpdateRegistrationAsync(id, dto);

                if (!success)
                    return NotFound(new { message = $"Inscripción con ID: {id} no encontrada." });

                return Ok(new { message = "Inscripción actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al actualizar la inscripción.", detail = ex.Message });
            }
        }

      
        [HttpPatch("{id:guid}/SoftDelete")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _registrationService.SoftDeleteToggleRegistrationAsync(id);

                if (!success)
                    return NotFound(new { message = $"No se encontró la inscripción con ID: {id}" });

                return Ok(new { message = "Estado de la inscripción (SoftDelete) actualizado con éxito." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al intentar eliminar la inscripción.", detail = ex.Message });
            }
        }

        #endregion

        #region GETS

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var registration = await _registrationService.GetRegistrationByIdAsync(id);
            if (registration == null) return NotFound(new { message = $"No se encontró la inscripción con ID: {id}" });

            return Ok(registration);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _registrationService.GetAllRegistrationsAsync();
            return Ok(result);
        }

        [HttpGet("search/tournament/{tournamentId:guid}")]
        
        public async Task<IActionResult> GetByTournamentId(Guid tournamentId)
        {
            var result = await _registrationService.GetRegistrationsByTournamentIdAsync(tournamentId);
            return Ok(result);
        }

        [HttpGet("search/category/{categoryId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            var result = await _registrationService.GetRegistrationsByCategoryIdAsync(categoryId);
            return Ok(result);
        }

        [HttpGet("search/couple/{coupleId:guid}")]
        public async Task<IActionResult> GetByCoupleId(Guid coupleId)
        {
            var result = await _registrationService.GetRegistrationsByCoupleIdAsync(coupleId);
            return Ok(result);
        }

        [HttpGet("search/date/{date}")]
        public async Task<IActionResult> GetByDate(DateOnly date)
        {
            var result = await _registrationService.GetRegistrationsByDateAsync(date);
            return Ok(result);
        }

        // endpoint para validar si la pareja ya se anotó
        [HttpGet("check-couple/{coupleId:guid}/category/{categoryId:guid}")]
        public async Task<IActionResult> CheckCoupleRegistration(Guid coupleId, Guid categoryId)
        {
            var isRegistered = await _registrationService.IsCoupleAlreadyRegisteredAsync(coupleId, categoryId);
            return Ok(new { isRegistered });
        }

        #endregion
    }
}