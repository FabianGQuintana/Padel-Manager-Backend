using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Venue;
using PadelManager.Application.Interfaces.Services;


namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        #region POST-PUT-PATCH (Escritura)

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Post([FromBody] CreateVenueDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _venueService.AddNewVenueAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la sede.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateVenueDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _venueService.UpdateVenueAsync(id, dto);
                if (!success) return NotFound(new { message = "Sede no encontrada." });

                return Ok(new { message = "Sede actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la sede.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _venueService.SoftDeleteToggleVenueAsync(id);
                if (!success) return NotFound(new { message = "Sede no encontrada." });

                return Ok(new { message = "Estado de la sede actualizado correctamente." });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                return Conflict(new { message = "No se puede modificar la sede debido a dependencias (posibles canchas activas).", detail = ex.InnerException?.Message });
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
            var result = await _venueService.GetAllVenuesAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _venueService.GetVenueByIdAsync(id);
            if (result == null) return NotFound(new { message = "Sede no encontrada." });
            return Ok(result);
        }

        [HttpGet("search/name/{name}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _venueService.GetVenuesByNameAsync(name);
            return Ok(result);
        }

        [HttpGet("search/city/{city}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetByCity(string city)
        {
            var result = await _venueService.GetVenuesByCityAsync(city);
            return Ok(result);
        }

        [HttpGet("search/address")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByAddress([FromQuery] string address)
        {
            var result = await _venueService.GetVenueByAddressAsync(address);
            if (result == null) return NotFound(new { message = "No se encontró ninguna sede con esa dirección." });
            return Ok(result);
        }

        #endregion
    }
}