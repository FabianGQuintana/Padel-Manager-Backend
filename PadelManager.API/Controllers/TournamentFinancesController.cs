using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.TournamentFinance;
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
    public class TournamentFinanceController : ControllerBase
    {
        private readonly ITournamentFinanceService _financeService;

        public TournamentFinanceController(ITournamentFinanceService financeService)
        {
            _financeService = financeService;
        }

        #region POST-PUT-PATCH (Escritura)

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Post([FromBody] CreateTournamentFinanceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _financeService.RegisterMovementAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar el movimiento financiero.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateTournamentFinanceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _financeService.UpdateMovementAsync(id, dto);
                if (!success) return NotFound(new { message = "Movimiento no encontrado." });

                return Ok(new { message = "Movimiento actualizado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el movimiento.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _financeService.SoftDeleteToggleFinanceAsync(id);
                if (!success) return NotFound(new { message = "Movimiento no encontrado." });

                return Ok(new { message = "Estado del registro financiero actualizado." });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                return Conflict(new { message = "Error de persistencia. Verifique dependencias.", detail = ex.InnerException?.Message });
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
            var result = await _financeService.GetAllFinancesAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _financeService.GetFinanceByIdAsync(id);
            if (result == null) return NotFound(new { message = "Registro financiero no encontrado." });
            return Ok(result);
        }

        [HttpGet("tournament/{tournamentId:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByTournament(Guid tournamentId)
        {
            var result = await _financeService.GetFinancesByTournamentIdAsync(tournamentId);
            return Ok(result);
        }

        [HttpGet("type/{type}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByType(TypeMovement type)
        {
            var result = await _financeService.GetFinancesByTypeAsync(type);
            return Ok(result);
        }

        // BALANCE NETO: Ingresos - Egresos
        [HttpGet("balance/tournament/{tournamentId:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetBalance(Guid tournamentId)
        {
            try
            {
                var balance = await _financeService.GetTotalBalanceByTournamentAsync(tournamentId);
                return Ok(new { tournamentId, balance, date = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al calcular el balance.", detail = ex.Message });
            }
        }

        #endregion
    }
}