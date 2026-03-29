using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Match;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Domain.Enum;
using System;
using System.Threading.Tasks;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //Lo ponemos aca para decirle a todos los metodos de la clase que son privados, no pueden acceder
    //Sin tener la autorizacion del token. Si queremos que algunos sean visibles para cualquier usuario ponemos [AllowAnonymous]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        #region PUT-PATCH-POST

        [HttpPost]
        //[Authorize] Haria falta poner en todos los metodos en este mismo lugar pero como lo puse de bajo del [ApiController] no hace falta
        public async Task<IActionResult> Post([FromBody] CreateMatchDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newMatch = await _matchService.AddNewMatchAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = newMatch.Id }, newMatch);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear el partido.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateMatchDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _matchService.UpdateMatchAsync(id, dto);

                if (!success)
                    return NotFound(new { message = $"Partido con ID: {id} no encontrado." });

                return Ok(new { message = "Partido actualizado con éxito." });
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
                var success = await _matchService.SoftDeleteToggleMatchAsync(id);

                if (!success)
                    return NotFound(new { message = $"No se encontró el partido con ID: {id}" });

                return Ok(new { Message = "Estado del partido actualizado con éxito." });
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

        // Lógica de Negocio: Cargar Resultado
        [HttpPatch("{id:guid}/LoadResult")]
        public async Task<IActionResult> LoadResult(Guid id, [FromBody] LoadMatchResultDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _matchService.LoadMatchResultAsync(id, dto);

                if (!success)
                    return NotFound(new { message = $"Partido con ID: {id} no encontrado." });

                return Ok(new { message = "Resultado cargado con éxito. Estado del partido cambiado a Finalizado." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al cargar el resultado.", detail = ex.Message });
            }
        }

        #endregion

        #region GETS

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);
            if (match == null) return NotFound($"No se encontro el partido con ID: {id}");

            return Ok(match);
        }

        [HttpGet]
        [AllowAnonymous] //Este por ejemplo , para que los users invitados puedan ver todos los partidos.
        public async Task<IActionResult> GetAll()
        {
            var result = await _matchService.GetAllMatchesAsync();
            return Ok(result);
        }

        [HttpGet("search/date/{date}")]
        public async Task<IActionResult> GetByDateAsync(DateTime date)
        {
            var result = await _matchService.GetMatchesByDateAsync(date);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search/status/{status}")]
        public async Task<IActionResult> GetByStatusAsync(MatchStatus status)
        {
            var result = await _matchService.GetMatchesByStatusAsync(status);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search/stage/{stageId:guid}")]
        public async Task<IActionResult> GetByStageIdAsync(Guid stageId)
        {
            var result = await _matchService.GetMatchesByStageIdAsync(stageId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search/zone/{zoneId:guid}")]
        public async Task<IActionResult> GetByZoneIdAsync(Guid zoneId)
        {
            var result = await _matchService.GetMatchesByZoneIdAsync(zoneId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search/couple/{coupleId:guid}")]
        public async Task<IActionResult> GetByCoupleIdAsync(Guid coupleId)
        {
            var result = await _matchService.GetMatchesByCoupleIdAsync(coupleId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search/location/{locationName}")]
        public async Task<IActionResult> GetByLocationAsync(string locationName)
        {
            var result = await _matchService.GetMatchesByLocationAsync(locationName);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search/court/{courtName}")]
        public async Task<IActionResult> GetByCourtAsync(string courtName)
        {
            var result = await _matchService.GetMatchesByCourtAsync(courtName);
            if (result == null) return NotFound();
            return Ok(result);
        }

        #endregion
    }
}

