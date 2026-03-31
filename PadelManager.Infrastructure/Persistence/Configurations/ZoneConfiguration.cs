using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
    {
        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            builder.ToTable("Zones");

            builder.HasKey(z => z.Id);

            // Filtro Global para Soft Delete
            builder.HasQueryFilter(z => z.DeletedAt == null);

            builder.Property(z => z.Name)
                .IsRequired()
                .HasMaxLength(100);

            // ==========================================
            // RELACIONES
            // ==========================================

            // 1. Relación con Stage (N:1) - Obligatoria
            builder.HasOne(z => z.Stage)
                .WithMany(s => s.Zones)
                .HasForeignKey(z => z.StageId)
                .OnDelete(DeleteBehavior.Restrict);


            // 2. Relación con Statistics (1:N)
            builder.HasMany(z => z.Statistics)
                .WithOne(s => s.Zone)
                .HasForeignKey(s => s.ZoneId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(z => z.Couples)
                .WithOne(c => c.Zone)
                .HasForeignKey(c => c.ZoneId)
                .OnDelete(DeleteBehavior.SetNull);


            // 3. Relación con Matches (1:N) - Opcional
            // Ya la configuramos en MatchConfiguration, pero EF la reconoce aquí también.
            builder.HasMany(z => z.Matches)
                .WithOne(m => m.Zone)
                .HasForeignKey(m => m.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}