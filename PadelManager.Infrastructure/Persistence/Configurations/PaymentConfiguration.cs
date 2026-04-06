using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            //  Precisión para el dinero (Copa Nea maneja montos grandes)
            builder.Property(p => p.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            //  Guardamos los Enums como String para que en pgAdmin se lea "Transfer" y no "2"
            builder.Property(p => p.PaymentMethod)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.PaymentStatus)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(p => p.Type)
                .HasConversion<string>()
                .IsRequired();

            // Relación: Un pago pertenece a una inscripción
            builder.HasOne(p => p.Registration)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
