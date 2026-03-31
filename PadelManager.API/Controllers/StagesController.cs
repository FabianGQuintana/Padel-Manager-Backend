using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Stage;
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
    public class StagesController : ControllerBase
    {
        private readonly IStageService _stageService;

        public StagesController(IStageService stageService)
        {
            _stageService = stageService;
        }

        #region POST-PUT-PATCH (Escritura)

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Post([FromBody] CreateStageDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _stageService.AddNewStageAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la etapa.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateStageDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _stageService.UpdateStageAsync(id, dto);
                if (!success) return NotFound(new { message = "Etapa no encontrada." });

                return Ok(new { message = "Etapa actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la etapa.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _stageService.SoftDeleteToggleStageAsync(id);
                if (!success) return NotFound(new { message = "Etapa no encontrada." });

                return Ok(new { message = "Estado de la etapa actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado.", detail = ex.Message });
            }
        }

        #endregion

        #region LÓGICA DE NEGOCIO

        [HttpPost("{id:guid}/generate-automatic")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GenerateAutomatic(Guid id)
        {
            try
            {
                var success = await _stageService.GenerateZonesAndMatchesAutomaticAsync(id);
                if (!success) return NotFound(new { message = "No se pudo encontrar la etapa para generar el sorteo." });

                return Ok(new { message = "Algoritmo de generación ejecutado con éxito." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error en el algoritmo de generación.", detail = ex.Message });
            }
        }

        #endregion

        #region GETS (Lectura)

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _stageService.GetAllStagesAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Tanteador")]
        public async Task<IActionResult> GetById(Guid id)
        {
            
            var result = await _stageService.GetStageByIdAsync(id);
            if (result == null) return NotFound(new { message = "Etapa no encontrada." });
            return Ok(result);
        }

        [HttpGet("category/{categoryId:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            var result = await _stageService.GetStagesByCategoryIdAsync(categoryId);
            return Ok(result);
        }

        [HttpGet("type/{type}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByType(StageType type)
        {
         
            var result = await _stageService.GetStagesByTypeAsync(type);
            return Ok(result);
        }

        #endregion
    }
}