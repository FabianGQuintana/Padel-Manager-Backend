using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByRegistrationIdAsync(Guid registrationId);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatusTypes status);
        Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(PaymentMethodTypes method);
        Task<IEnumerable<Payment>> GetPaymentsByDateAsync(DateTime date);
        Task<IEnumerable<Payment>> GetPaymentsByAmountAsync(decimal amount);
    }
}
