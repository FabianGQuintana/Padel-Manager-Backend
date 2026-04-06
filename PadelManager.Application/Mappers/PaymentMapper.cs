using PadelManager.Application.DTOs.Payment;
using PadelManager.Domain.Entities;
using PadelManager.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Application.Mappers
{
    namespace PadelManager.Application.Mappers
    {
        public static class PaymentMapper
        {
            public static PaymentResponseDto ToResponseDto(this Payment payment)
            {
                return new PaymentResponseDto
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod.ToString(),
                    PaymentStatus = payment.PaymentStatus.ToString(),
                    Type = payment.Type.ToString(),
                    RegistrationId = payment.RegistrationId
                };
            }

            public static IEnumerable<PaymentResponseDto> ToResponseDto(this IEnumerable<Payment> payments)
            {
                return payments.Select(p => p.ToResponseDto());
            }

            public static void MapToEntity(this Payment existingEntity, UpdatePaymentDto dto)
            {
                if (dto.Amount.HasValue) existingEntity.Amount = dto.Amount.Value;
                if (dto.PaymentMethod.HasValue) existingEntity.PaymentMethod = dto.PaymentMethod.Value;
                if (dto.PaymentStatus.HasValue) existingEntity.PaymentStatus = dto.PaymentStatus.Value;
                if (dto.Type.HasValue) existingEntity.Type = dto.Type.Value;
            }

            public static Payment ToEntity(this CreatePaymentDto dto)
            {
                return new Payment
                {
                    Amount = dto.Amount,
                    PaymentMethod = dto.PaymentMethod,
                    Type = dto.Type,
                    RegistrationId = dto.RegistrationId,
                    PaymentStatus = PaymentStatusTypes.Pending,
                    PaymentDate = DateTime.UtcNow
                };
            }
        }
    }
}
