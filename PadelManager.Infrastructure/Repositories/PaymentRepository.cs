using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using PadelManager.Infrastructure.Persistence;

namespace PadelManager.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(PadelManagerDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetPaymentsByRegistrationIdAsync(Guid registrationId)
        {
            return await _context.Payments
                .Include(p => p.Registration)
                .Where(p => p.RegistrationId == registrationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatusTypes status)
        {

            return await _context.Payments
                .Where(p => p.PaymentStatus == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(PaymentMethodTypes method)
        {
            return await _context.Payments
                .Where(p => p.PaymentMethod == method)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateAsync(DateTime date)
        {
            return await _context.Payments
                .Where(p => p.PaymentDate.Date == date.Date) 
                .ToListAsync();
        }


        public async Task<IEnumerable<Payment>> GetPaymentsByAmountAsync(decimal amount)
        {
            return await _context.Payments
                .Where(p => p.Amount == amount)
                .ToListAsync();
        }

    }
}
