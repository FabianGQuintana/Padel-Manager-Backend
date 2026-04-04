using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);


            // ==========================================
            // CONFIGURACIÓN DE PROPIEDADES
            // ==========================================

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            // ==========================================
            // No pueden haber dos categorías con el mismo nombre en el mismo torneo.
            // Ejemplo: No podés tener dos "5ta Damas" en el Torneo de Verano.
            // ==========================================
            builder.HasIndex(c => new { c.Name, c.TournamentId })
                   .IsUnique();

            builder.Property(c => c.MaxTeams).IsRequired();

            // ==========================================
            // RELACIONES
            // ==========================================

            // 1. Relación con Tournament (N:1) - Obligatoria
            builder.HasOne(c => c.Tournament)
                .WithMany(t => t.Categories)
                .HasForeignKey(c => c.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
            // Si borras el torneo, se borran todas sus categorías.

            // 2. Relación con Stage (1:N)
            builder.HasMany(c => c.Stages)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. Relación con Registration (1:N)
            builder.HasMany(c => c.Registrations)
                .WithOne(r => r.Category)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}