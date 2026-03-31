using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Zone;
using PadelManager.Application.Interfaces.Services;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ZonesController : ControllerBase
    {
        private readonly IZoneService _zoneService;

        public ZonesController(IZoneService zoneService)
        {
            _zoneService = zoneService;
        }


        
        #region PUT-POST-PATCH
        [HttpPost]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Post([FromBody] CreateZoneDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newZone = await _zoneService.AddZoneAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = newZone.Id }, newZone);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear la zona.", detail = ex.Message });
            }

        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id,[FromBody] UpdateZoneDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _zoneService.UpdateZoneAsync(id, dto);
                if (!success)
                    return NotFound(new { message = $"Zona con ID: {id} no encontrado." });

                return Ok(new { message = "Zona actualizada con éxito." });
            }catch (InvalidOperationException ex)
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
                var success = await _zoneService.SoftDeleteZoneAsync(id);
                if (!success)
                    return NotFound(new { message = $"No se encontró la zona con ID: {id}" });

                return Ok(new { Message = "Estado de la zona:  actualizada con éxito." });

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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _zoneService.GetAllZonesAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _zoneService.GetZoneByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"No se encontró la zona con ID: {id}" });

            return Ok(result);
        }

        [HttpGet("{id:guid}/details")]
        public async Task<IActionResult> GetWithDetails(Guid id)
        {
           
            var result = await _zoneService.GetZoneWithDetailsAsync(id);

            if (result == null)
                return NotFound(new { message = "No se encontraron detalles para esta zona." });

            return Ok(result);
        }

        [HttpGet("search/name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _zoneService.GetZonesByNameAsync(name);
            return Ok(result);
        }

        #endregion

        #region LÓGICA DE NEGOCIO (SORTEO)

        [HttpPost("generate-draw/{categoryId:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GenerateDraw(Guid categoryId)
        {
            try
            {
                var success = await _zoneService.GenerateZonesWithDrawAsync(categoryId);

                if (!success)
                    return BadRequest(new { message = "No se pudieron generar las zonas. Verifique que la categoría tenga un Stage de grupos activo." });

                return Ok(new { message = "Sorteo realizado y zonas generadas con éxito." });
            }
            catch (Exception ex)
            {
                // Capturamos el "No hay suficientes parejas" o cualquier error del algoritmo
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}
