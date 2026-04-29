using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Payment;
using PadelManager.Application.DTOs.Tournament;
using PadelManager.Domain.Enum; // Necesario para el Enum de Status

namespace PadelManager.API.Controllers

{

    [Route("api/[controller]")]

    [ApiController]
    [Authorize] //Lo ponemos aca para decirle a todos los metodos de la clase que son privados, no pueden acceder
    //Sin tener la autorizacion del token. Si queremos que algunos sean visibles para cualquier usuario ponemos [AllowAnonymous]
    public class TournamentsController : ControllerBase

    {

        private readonly ITournamentService _tournamentService;



        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        #region POST - PUT - PATCH

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Post([FromBody] CreateTournamentDto dto)

        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newTournament = await _tournamentService.AddNewTournamentAsync(dto);

                // Retorna 201 Created y la URL para acceder al recurso recién creado

                return CreatedAtAction(nameof(GetById), new { id = newTournament.Id }, newTournament);
            }

            catch (InvalidOperationException ex)

            {
                return BadRequest(new { message = ex.Message });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al crear el torneo.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateTournamentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _tournamentService.UpdateTournamentAsync(id, dto);

                if (!success)

                    return NotFound(new { message = $"Torneo con ID: {id} no encontrado." });

                return Ok(new { message = "Torneo actualizado con éxito." });

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
                var success = await _tournamentService.SoftDeleteToggleTournamentAsync(id);

                if (!success)
                    return NotFound(new { message = $"No se encontró el torneo con ID: {id}" });

                return Ok(new { message = "Estado del torneo actualizado con éxito." });
            }
            
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado en el servidor al intentar cambiar el estado.",
                    detail = ex.Message
                });
            }
        }


        [HttpPatch("{id:guid}/Start")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> StartTournament(Guid id)
        {
            try
            {

                var success = await _tournamentService.CloseRegistrationsAndStartAsync(id);

                if (!success)

                    return NotFound(new { message = $"No se encontró el torneo con ID: {id}" });

                return Ok(new { message = "Torneo iniciado con éxito. Las inscripciones han sido cerradas." });

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al intentar iniciar el torneo.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/OpenRegistrations")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> OpenRegistrations(Guid id)
        {
            try
            {
                var success = await _tournamentService.OpenTournamentRegistrationsAsync(id);

                if (!success)
                    return NotFound(new { message = $"No se encontró el torneo con ID: {id}" });

                return Ok(new { message = "¡Inscripciones abiertas! El torneo ya es visible para los jugadores." });
            }
            catch (InvalidOperationException ex)
            {
                //  Aquí es donde React recibe el mensaje de "Faltan categorías"
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al intentar abrir las inscripciones.", detail = ex.Message });
            }
        }

        #endregion

        #region GETS
        [HttpGet("{id:guid}")]
        [AllowAnonymous] // para permitir que cualquiera vea el detalle de un torneo
        public async Task<IActionResult> GetById(Guid id)
        {
            var tournament = await _tournamentService.GetTournamentByIdAsync(id);

            if (tournament == null) return NotFound(new { message = $"No se encontró el torneo con ID: {id}" });

            return Ok(tournament);

        }

        [HttpGet]
        [AllowAnonymous] 

        public async Task<IActionResult> GetAll()
        {
            var result = await _tournamentService.GetAllTournamentsAsync();
            return Ok(result);
        }

        [HttpGet("search/name/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _tournamentService.GetTournamentByNameAsync(name);
            return Ok(result);
        }


        [HttpGet("search/status/{status}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByStatus(TournamentStatus status)
        {
            var result = await _tournamentService.GetTournamentsByStatusAsync(status);
            return Ok(result);
        }

        [HttpGet("search/type/{type}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByType(string type)
        {
            var result = await _tournamentService.GetTournamentsByTypeAsync(type);
            return Ok(result);
        }



        [HttpGet("search/date/{startDate}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByStartDate(DateTime startDate)
        {
           var result = await _tournamentService.GetTournamentsByStartDateAsync(startDate);
            return Ok(result);
        }

        // GETS POR MANAGER
        [HttpGet("search/manager/{managerId:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByManagerId(Guid managerId)
        {
            var result = await _tournamentService.GetTournamentsByManagerIdAsync(managerId);
            return Ok(result);
        }


        [HttpGet("search/manager/email/{email}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByManagerEmail(string email)
        {
            var result = await _tournamentService.GetTournamentsByManagerEmailAsync(email);
            return Ok(result);
        }

        [HttpGet("search/manager/dni/{dni}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByManagerDni(string dni)
        {
            var result = await _tournamentService.GetTournamentsByManagerDniAsync(dni);
            return Ok(result);
        }

        [HttpGet("search/manager/name/{name}")]
        [Authorize(Roles = "Admin, Organizador")]

        public async Task<IActionResult> GetByManagerName(string name)
        {
            var result = await _tournamentService.GetTournamentsByManagerNameAsync(name);
            return Ok(result);
        }

        #endregion

        #region INFORMACIÓN FINANCIERA Y PAGOS

        [HttpGet("{id:guid}/financial-summary")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetFinancialSummary(Guid id)
        {
            try
            {
                var summary = await _tournamentService.GetFinancialSummaryAsync(id);

                if (summary == null)
                    return NotFound(new { message = $"No se encontró información contable para el torneo con ID: {id}" });

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al generar el resumen financiero.",
                    detail = ex.Message
                });
            }
        }

        [HttpGet("{id:guid}/payment-grids")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetPaymentGrids(Guid id)
        {
            try
            {
                var result = await _tournamentService.GetCategoryPaymentGridsAsync(id);

                // Si la lista viene vacía, verificamos si es que el torneo no existe
                if (result == null || !result.Any())
                {
                    // Podrías hacer un chequeo extra aquí o simplemente devolver la lista vacía
                    return Ok(result ?? new List<CategoryPaymentGridDto>());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error al recuperar las grillas de pago por categoría.",
                    detail = ex.Message
                });
            }
        }

        #endregion
    }

}