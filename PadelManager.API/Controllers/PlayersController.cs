using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Player;
using PadelManager.Application.Interfaces.Services;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        #region PUT-PATCH-POST

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Post([FromBody] CreatePlayerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newPlayer = await _playerService.AddNewPlayerAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = newPlayer.Id }, newPlayer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear el jugador.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdatePlayerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _playerService.UpdatePlayerAsync(id, dto);

                if (!success)
                    return NotFound(new { message = $"Jugador con ID: {id} no encontrado." });

                return Ok(new { message = "Jugador actualizado con éxito." });
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
                var success = await _playerService.SoftDeleteTogglePlayerAsync(id);

                if (!success)
                    return NotFound(new { message = $"No se encontró el jugador con ID: {id}" });

                return Ok(new { Message = "Estado del jugador actualizado con éxito." });
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

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _playerService.GetPlayerByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _playerService.GetAllPlayersAsync();
            return Ok(result);
        }

        [HttpGet("search/name/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _playerService.GetPlayerByNameAsync(name);
            return Ok(result);
        }

        [HttpGet("search/lastname/{lastName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByLastName(string lastName)
        {
            var result = await _playerService.GetPlayerByLastNameAsync(lastName);
            return Ok(result);
        }

        [HttpGet("search/phone/{phoneNumber}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByPhoneNumber(string phoneNumber)
        {
            var result = await _playerService.GetPlayerByPhoneNumberAsync(phoneNumber);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search/dni/{dni}")]
        [Authorize(Roles = "Admin, Organizador,Jugador")]
        public async Task<IActionResult> GetByDni(string dni)
        {
            var result = await _playerService.GetPlayerByDniAsync(dni);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search/age/{age}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByAge(byte age)
        {
            var result = await _playerService.GetPlayersByAgeAsync(age);
            return Ok(result);
        }

        [HttpGet("search/availability/{availability}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetByAvailability(string availability)
        {
            var result = await _playerService.GetPlayersByAvailabilityAsync(availability);
            return Ok(result);
        }

        #endregion
    }
}
