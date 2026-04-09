using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelManager.Application.DTOs.Payment;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Domain.Enum;


namespace PadelManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        #region POST-PUT-PATCH (Escritura)

        [HttpPost]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> Post([FromBody] CreatePaymentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _paymentService.RegisterPaymentAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar el pago.", detail = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdatePaymentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var success = await _paymentService.UpdatePaymentAsync(id, dto);
                if (!success) return NotFound(new { message = "Pago no encontrado." });

                return Ok(new { message = "Pago actualizado con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el pago.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/status")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] PaymentStatusTypes status)
        {
            try
            {
                var success = await _paymentService.ChangePaymentStatusAsync(id, status);
                if (!success) return NotFound(new { message = "Pago no encontrado." });

                return Ok(new { message = $"Estado del pago cambiado a {status}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al cambiar el estado del pago.", detail = ex.Message });
            }
        }

        [HttpPatch("{id:guid}/SoftDelete")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            try
            {
                var success = await _paymentService.SoftDeleteTogglePaymentAsync(id);
                if (!success) return NotFound(new { message = "Pago no encontrado." });

                return Ok(new { message = "Estado de auditoría del pago actualizado." });
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
            var result = await _paymentService.GetAllPaymentAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _paymentService.GetPaymentByIdAsync(id);
            if (result == null) return NotFound(new { message = "Pago no encontrado." });
            return Ok(result);
        }

        [HttpGet("registration/{registrationId:guid}")]
        [Authorize(Roles = "Admin, Organizador, Jugador")]
        public async Task<IActionResult> GetByRegistration(Guid registrationId)
        {
            var result = await _paymentService.GetPaymentsByRegistrationIdAsync(registrationId);
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByStatus(PaymentStatusTypes status)
        {
            var result = await _paymentService.GetPaymentsByStatusAsync(status);
            return Ok(result);
        }

        [HttpGet("method/{method}")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByMethod(PaymentMethodTypes method)
        {
            var result = await _paymentService.GetPaymentsByMethodAsync(method);
            return Ok(result);
        }

        [HttpGet("search/date")]
        [Authorize(Roles = "Admin, Organizador")]
        public async Task<IActionResult> GetByDate([FromQuery] string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                var result = await _paymentService.GetPaymentsByDateAsync(parsedDate);
                return Ok(result);
            }
            return BadRequest(new { message = "Formato de fecha inválido." });
        }

        #endregion
    }
}