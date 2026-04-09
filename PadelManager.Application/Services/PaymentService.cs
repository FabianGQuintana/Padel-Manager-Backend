using PadelManager.Application.DTOs.Payment;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Application.Mappers;
using PadelManager.Application.Mappers.PadelManager.Application.Mappers;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PadelManager.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public PaymentService(
            IPaymentRepository paymentRepo,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _paymentRepo = paymentRepo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        // ==========================================
        // COMANDOS (ESCRITURA)
        // ==========================================

        public async Task<PaymentResponseDto> RegisterPaymentAsync(CreatePaymentDto dto)
        {
            var payment = dto.ToEntity();

            // AUDITORÍA
            var user = _currentUser.UserName ?? "System";
            payment.CreatedBy = user;
            payment.LastModifiedBy = user;

            await _paymentRepo.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return payment.ToResponseDto();
        }

        public async Task<bool> UpdatePaymentAsync(Guid paymentId, UpdatePaymentDto dto)
        {
            var existingPayment = await _paymentRepo.GetByIdAsync(paymentId);
            if (existingPayment == null) return false;

            // Usamos el Mapper para actualizaciones parciales
            existingPayment.MapToEntity(dto);

            // AUDITORÍA
            existingPayment.LastModifiedBy = _currentUser.UserName ?? "System";
            existingPayment.LastModifiedAt = DateTime.UtcNow;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePaymentStatusAsync(Guid paymentId, PaymentStatusTypes newStatus)
        {
            // 1. Buscamos el pago
            var payment = await _paymentRepo.GetByIdAsync(paymentId);
            if (payment == null) return false;

            // 2. Aplicamos el cambio
            payment.PaymentStatus = newStatus;

            // 3. AUDITORÍA (Siguiendo tu estándar)
            payment.LastModifiedBy = _currentUser.UserName ?? "System";
            payment.LastModifiedAt = DateTime.UtcNow;

            // 4. PERSISTENCIA
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteTogglePaymentAsync(Guid paymentId)
        {
            var payment = await _paymentRepo.GetByIdAsync(paymentId);
            if (payment == null) return false;

            // AUDITORÍA
            payment.LastModifiedBy = _currentUser.UserName ?? "System";
            payment.LastModifiedAt = DateTime.UtcNow;

            await _paymentRepo.SoftDeleteToggleAsync(paymentId);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        // ==========================================
        // CONSULTAS (LECTURA)
        // ==========================================

        public async Task<PaymentResponseDto?> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            return payment?.ToResponseDto();
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetAllPaymentAsync()
        {
            var payments = await _paymentRepo.GetAllAsync();
            return payments.ToResponseDto();
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByRegistrationIdAsync(Guid registrationId)
        {
            var payments = await _paymentRepo.GetPaymentsByRegistrationIdAsync(registrationId);
            return payments.ToResponseDto();
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByDateAsync(DateTime date)
        {
            // Buscamos pagos en el día específico
            var payments = await _paymentRepo.GetPaymentsByDateAsync(date);
            return payments.ToResponseDto();
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByAmountAsync(decimal amount)
        {
            var payments = await _paymentRepo.GetPaymentsByAmountAsync(amount);
            return payments.ToResponseDto();
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByMethodAsync(PaymentMethodTypes method)
        {
            var payments = await _paymentRepo.GetPaymentsByMethodAsync(method);
            return payments.ToResponseDto();
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStatusAsync(PaymentStatusTypes status)
        {
            var payments = await _paymentRepo.GetPaymentsByStatusAsync(status);
            return payments.ToResponseDto();
        }
    }
}