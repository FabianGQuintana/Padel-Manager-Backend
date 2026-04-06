using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PadelManager.Application.DTOs.Statistic;
using PadelManager.Application.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticsService;

        public StatisticsController(IStatisticService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        #region PUT-PATCH-POST

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Post([FromBody] CreateStatisticDto dto)
        {
            try
            {
                var newStatistics = await _statisticsService.AddNewStatisticAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newStatistics.Id }, newStatistics);
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { message = "Error de integridad: Posible duplicado o datos relacionados inexistentes.", detail = ex.InnerException?.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear la estadística.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateStatisticDto dto)
        {
            try
            {
                var success = await _statisticsService.UpdateStatisticAsync(id, dto);

                if (!success)
                    return NotFound(new { message = $"Estadística con ID: {id} no encontrada." });

                return Ok(new { message = "Estadística actualizada con éxito." });
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { message = "Error al actualizar: conflicto de datos en el servidor.", detail = ex.InnerException?.Message });
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
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _statisticsService.SoftDeleteToggleStatisticAsync(id);

                if (!success)
                    return NotFound(new { message = $"No se encontró la estadística con ID: {id}" });

                return Ok(new { Message = "Estado de la estadística actualizado con éxito." });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (DbUpdateException ex)
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _statisticsService.GetStatisticByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _statisticsService.GetAllStatisticsAsync();
            return Ok(result);
        }

        [HttpGet("search/couple/{coupleId:guid}")]
        public async Task<IActionResult> GetByCoupleId(Guid coupleId)
        {
            var result = await _statisticsService.GetStatisticsByCoupleIdAsync(coupleId);
            return Ok(result);
        }

        [HttpGet("search/zone/{zoneId:guid}")]
        public async Task<IActionResult> GetByZoneId(Guid zoneId)
        {
            var result = await _statisticsService.GetStatisticsByZoneIdAsync(zoneId);
            return Ok(result);
        }

        [HttpGet("search/couple/{coupleId:guid}/zone/{zoneId:guid}")]
        public async Task<IActionResult> GetByCoupleIdAndZoneId(Guid coupleId, Guid zoneId)
        {
            var result = await _statisticsService.GetStatisticByCoupleIdAndZoneIdAsync(coupleId, zoneId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        #endregion
    }
}