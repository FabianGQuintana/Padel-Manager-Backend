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
            
            builder.HasOne(s => s.Category)
                .WithMany(c => c.Stages)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Relación con Zone (1:N)
            
            builder.HasMany(s => s.Zones)
                .WithOne(z => z.Stage)
                .HasForeignKey(z => z.StageId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Relación con Match (1:N)
           
            builder.HasMany(s => s.Matches)
                .WithOne(m => m.Stage)
                .HasForeignKey(m => m.StageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}