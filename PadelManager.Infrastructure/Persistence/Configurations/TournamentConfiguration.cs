using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder.ToTable("Tournaments");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.StartDate)
                .IsRequired();

            builder.Property(t => t.Regulations)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(t => t.StatusType)
                .IsRequired();

            builder.Property(t => t.TournamentType)
                .IsRequired()
                .HasMaxLength(50);

            // ==========================================
            // RELACIONES
            // ==========================================

            // 1. Relación con Manager (N:M)
            builder.HasMany(t => t.Managers)
           .WithMany(m => m.Tournaments)
           .UsingEntity(j => j.ToTable("TournamentManagers")); // Nombre de la tabla de unión

            // 2. Relación con Category (1:N)
            // Un Torneo tiene muchas categorías.
            builder.HasMany(t => t.Categories)
                .WithOne(c => c.Tournament)
                .HasForeignKey(c => c.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
