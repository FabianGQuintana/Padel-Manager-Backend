using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;
using System;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matches");

            builder.HasKey(m => m.Id);

            // ==========================================
            // CONFIGURACIÓN DE PROPIEDADES
            // ==========================================


            // Mapeo del Enum como String (más fácil de leer en pgAdmin)
            builder.Property(m => m.StatusType)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(m => m.LocationName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(m => m.CourtName)
                   .HasMaxLength(50)
                   .IsRequired();

            // Los IDs de ganador y perdedor pueden ser nulos al inicio
            builder.Property(m => m.WinnerCoupleId)
                   .IsRequired(false);

            builder.Property(m => m.LoserCoupleId)
                   .IsRequired(false);

            // ==========================================
            // RELACIONES
            // ==========================================

            // 1. Relación con Stage (Obligatoria)
            builder.HasOne(m => m.Stage)
                   .WithMany(s => s.Matches)
                   .HasForeignKey(m => m.StageId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Si se borra la etapa (ej. "8vos"), se borran sus partidos.

            // 2. Relación con Zone (Opcional)
            builder.HasOne(m => m.Zone)
                   .WithMany(z => z.Matches)
                   .HasForeignKey(m => m.ZoneId)
                   .OnDelete(DeleteBehavior.Restrict);
            // Restrict para evitar borrar zonas por accidente si tienen partidos. 

            // 3. Relación con Pareja A (Couple)
            builder.HasOne(m => m.Couple)
                   .WithMany() // Couple no necesita una lista de "Partidos donde soy Pareja A"
                   .HasForeignKey(m => m.CoupleId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 4. Relación con Pareja B (Couple2)
            builder.HasOne(m => m.Couple2)
                   .WithMany()
                   .HasForeignKey(m => m.CoupleId2)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}