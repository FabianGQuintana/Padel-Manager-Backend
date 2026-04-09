using PadelManager.Application.DTOs.Payment;
using PadelManager.Domain.Enum;


namespace PadelManager.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        //CRUD
        Task<PaymentResponseDto> RegisterPaymentAsync(CreatePaymentDto dto);
        // Para actualizaciones generales (monto, método, tipo)
        Task<bool> UpdatePaymentAsync(Guid paymentId, UpdatePaymentDto dto);

        // si solo quiere cambiar el estado rápido (muy común en finanzas)
        Task<bool> ChangePaymentStatusAsync(Guid paymentId, PaymentStatusTypes newStatus);
        Task<bool> SoftDeleteTogglePaymentAsync(Guid paymentId);
        
        //GETS
        Task<PaymentResponseDto?> GetPaymentByIdAsync(Guid id);
        Task<IEnumerable<PaymentResponseDto>> GetAllPaymentAsync();
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByRegistrationIdAsync(Guid registrationId);
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByDateAsync(DateTime date);
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByAmountAsync(decimal amount);
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByMethodAsync(PaymentMethodTypes method);
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStatusAsync(PaymentStatusTypes status);

    }
}
