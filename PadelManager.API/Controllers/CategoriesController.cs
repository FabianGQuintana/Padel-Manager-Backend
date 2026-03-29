using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Category;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Services;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //Lo ponemos aca para decirle a todos los metodos de la clase que son privados, no pueden acceder
    //Sin tener la autorizacion del token. Si queremos que algunos sean visibles para cualquier usuario ponemos [AllowAnonymous]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        #region PUT-PATCH-POST

        [HttpPost]
        //[Authorize] Haria falta poner en todos los metodos en este mismo lugar pero como lo puse de bajo del [ApiController] no hace falta
        public async Task<IActionResult> Post([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newCategory = await _categoryService.AddNewCategoryAsync(dto);

                return CreatedAtAction(nameof(GetById), new { id = newCategory.Id }, newCategory);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear la categoria.", detail = ex.Message });
            }
        }


        [HttpPut("{id:guid}")]  
        public async Task<IActionResult> Put(Guid id,  [FromBody] UpdateCategoryDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {

                var success = await _categoryService.UpdateCategoryAsync(id,dto);

                if (!success)
                    return NotFound(new { message = $"Categoria con ID: {id} no encontrado." });

                return Ok(new { message = "Categoria actualizada con éxito." });
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
                var success = await _categoryService.SoftDeleteToggleCategoryAsync(id);

                if (!success)
                    return NotFound(new { message = $"No se encontró la categoria con ID: {id}" });

                return Ok(new { Message = "Estado de la categoria:  actualizada con éxito." });
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
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null ) return NotFound($"No se encontro la categoria con ID: {id}");

            return Ok(category);
        }


        [HttpGet]
        [AllowAnonymous] //Este por ejemplo , para que los users invitados puedan ver todas las categorias.
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllCategoriesAsync();

            return Ok(result);

        }


        [HttpGet("search/{id:guid}/WithRegistration")]
        public async Task<IActionResult> GetWithRegistrations(Guid id)
        {
            var result = await _categoryService.GetCategoryWithRegistrationsAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet("search/{id:guid}/WithTournament")]
        public async Task<IActionResult> GetTournamentIdAsync(Guid id)
        {
            var result = await _categoryService.GetCategoriesByTournamentIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet("search/name/{name}")]
        public async Task<IActionResult> GetNameAsync(string name)
        {
            var result = await _categoryService.GetCategoriesByNameAsync(name);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpGet("search/maxTeams/{maxTeams:int}")]
        public async Task<IActionResult> GetMaxTeamsAsync(int maxTeams)
        {
            var result = await _categoryService.GetCategoriesByMaxTeamsAsync(maxTeams);
            if (result == null) return NotFound();
            return Ok(result);
        }

  
        #endregion

    }
}
