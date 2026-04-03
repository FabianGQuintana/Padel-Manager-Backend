using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class StatisticConfiguration : IEntityTypeConfiguration<Statistic>
    {
        public void Configure(EntityTypeBuilder<Statistic> builder)
        {
            builder.ToTable("Statistics");

            builder.HasKey(s => s.Id);


            // ==========================================
            // CONFIGURACIÓN DE PROPIEDADES
            // ==========================================

            // Nos aseguramos de que todos los contadores empiecen en 0
            builder.Property(s => s.Points).HasDefaultValue(0);
            builder.Property(s => s.WoCount).HasDefaultValue(0);
            builder.Property(s => s.MatchesPlayed).HasDefaultValue(0);
            builder.Property(s => s.MatchesWon).HasDefaultValue(0);
            builder.Property(s => s.SetsWon).HasDefaultValue(0);
            builder.Property(s => s.SetsLost).HasDefaultValue(0);
            builder.Property(s => s.GamesWon).HasDefaultValue(0);
            builder.Property(s => s.GamesLost).HasDefaultValue(0);

            // ==========================================
            // Una pareja solo puede tener UN registro de estadística por Zona.
            // ==========================================
            builder.HasIndex(s => new { s.CoupleId, s.ZoneId })
                   .IsUnique();

            // ==========================================
            // RELACIONES
            // ==========================================

            // 1. Relación con Couple (N:1)
            builder.HasOne(s => s.Couple)
                   .WithMany() // No necesitamos una lista de estadísticas en Couple
                   .HasForeignKey(s => s.CoupleId)
                   .OnDelete(DeleteBehavior.Restrict);
            // No borramos la pareja si se borra su estadística.

            // 2. Relación con Zone (N:1)
            builder.HasOne(s => s.Zone)
                   .WithMany(z => z.Statistics)
                   .HasForeignKey(s => s.ZoneId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Si se borra la zona, se borran sus estadísticas (cascada lógica).
        }
    }
}