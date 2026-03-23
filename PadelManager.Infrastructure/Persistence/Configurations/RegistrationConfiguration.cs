using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;
using System;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.HasQueryFilter(t => t.DeletedAt == null);

            // Regla de Oro: Índice único compuesto
            // Evita que la Pareja X se inscriba dos veces en la Categoría Y
            builder.HasIndex(r => new { r.CoupleId, r.CategoryId })
                   .IsUnique();

            // Fecha y hora por defecto si no se envían
            builder.Property(r => r.RegistrationDate)
                   .IsRequired();

            builder.Property(r => r.RegistrationTime)
                   .IsRequired();

            // Relaciones
            builder.HasOne(r => r.Couple)
                   .WithMany(c => c.Registrations)
                   .HasForeignKey(r => r.CoupleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Category)
                   .WithMany(c => c.Registrations)
                   .HasForeignKey(r => r.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Tournament)
                   .WithMany(t => t.Registrations)
                   .HasForeignKey(r => r.TournamentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
