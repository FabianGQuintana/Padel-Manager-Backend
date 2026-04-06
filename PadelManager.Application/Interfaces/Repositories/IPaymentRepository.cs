using PadelManager.Domain.Entities;

namespace PadelManager.Application.Interfaces.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByRegistrationIdAsync(Guid registrationId);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);
        Task<IEnumerable<Payment>> GetPaymentsByMethodPayAsync(string method);
        Task<IEnumerable<Payment>> GetPaymentsByDateAsync(DateTime date);
        Task<IEnumerable<Payment>> GetPaymentsByAmountAsync(decimal amount);
    }
}
