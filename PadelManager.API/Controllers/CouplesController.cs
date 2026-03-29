using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Couple;
using PadelManager.Application.Interfaces.Services;

namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouplesController : ControllerBase
    {
        private readonly ICoupleService _coupleService;

        public CouplesController(ICoupleService coupleService)
        {
            _coupleService = coupleService;
        }

        #region PUT-PATCH-POST

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCoupleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var newCouple = await _coupleService.AddNewCoupleAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newCouple.Id }, newCouple);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la pareja.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCoupleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _coupleService.UpdateCoupleAsync(id, dto);
            if (!success) return NotFound();

            return Ok(new { message = "Pareja actualizada correctamente." });
        }

        [HttpPatch("{id:guid}/replace-player")]
        public async Task<IActionResult> ReplacePlayer(Guid id, [FromBody] ReplaceCouplePlayerDto dto)
        {
            try
            {
                var success = await _coupleService.ReplacePlayerInCoupleAsync(id, dto);
                if (!success) return BadRequest(new { message = "No se pudo realizar el reemplazo." });

                return Ok(new { message = "Jugador reemplazado con éxito." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var success = await _coupleService.SoftDeleteToggleCoupleAsync(id);
            if (!success) return NotFound();
            return Ok(new { message = "Estado de la pareja actualizado." });
        }

        #endregion

        #region GETS

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var couple = await _coupleService.GetCoupleByIdAsync(id);
            if (couple == null) return NotFound();
            return Ok(couple);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _coupleService.GetAllCouplesAsync());
        }

        [HttpGet("search/nickname/{nickname}")]
        public async Task<IActionResult> GetByNickname(string nickname)
        {
            var couple = await _coupleService.GetCoupleByNicknameAsync(nickname);
            if (couple == null) return NotFound();
            return Ok(couple);
        }

        [HttpGet("search/player/{playerId:guid}")]
        public async Task<IActionResult> GetByPlayer(Guid playerId)
        {
            return Ok(await _coupleService.GetCouplesByPlayerIdAsync(playerId));
        }

        [HttpGet("no-zone")]
        public async Task<IActionResult> GetWithoutZone()
        {
            return Ok(await _coupleService.GetCouplesWithoutZoneAsync());
        }

        #endregion
    }
}