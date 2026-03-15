using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class StageConfiguration : IEntityTypeConfiguration<Stage>
    {
        public void Configure(EntityTypeBuilder<Stage> builder)
        {
            builder.ToTable("Stages");

            builder.HasKey(s => s.Id);

            // Filtro para Soft Delete: No trae etapas borradas lógicamente
            builder.HasQueryFilter(s => s.DeletedAt == null);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Mapeo del Enum StageType como string para mayor claridad en la DB
            builder.Property(s => s.Type)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(s => s.Order)
                .IsRequired();

            // ==========================================
            // RELACIONES
            // ==========================================

            // 1. Relación con Category (N:1) - Obligatoria
            // Si se borra la categoría (ej: 4ta), se borran todas sus etapas (8vos, 4tos, etc.)
            builder.HasOne(s => s.Category)
                .WithMany(c => c.Stages)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Relación con Zone (1:N)
            // Ya la definimos en ZoneConfiguration, pero aquí confirmamos el vínculo
            builder.HasMany(s => s.Zones)
                .WithOne(z => z.Stage)
                .HasForeignKey(z => z.StageId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. Relación con Match (1:N)
            // Lo mismo para los partidos
            builder.HasMany(s => s.Matches)
                .WithOne(m => m.Stage)
                .HasForeignKey(m => m.StageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}