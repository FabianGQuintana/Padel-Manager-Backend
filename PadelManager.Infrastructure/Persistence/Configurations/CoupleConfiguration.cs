using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class CoupleConfiguration : IEntityTypeConfiguration<Couple>
    {
        public void Configure(EntityTypeBuilder<Couple> builder)
        {
            builder.ToTable("Couples");

            builder.HasKey(c => c.Id);


            builder.Property(c => c.Nickname)
                .HasMaxLength(50)
                .IsRequired(false);

            // ============================================================
            //  REGLA DE ORO: Unicidad de la Pareja
            // Evita que se cree la misma combinación de jugadores dos veces.
            // ============================================================
            builder.HasIndex(c => new { c.Player1Id, c.Player2Id })
                .IsUnique();

            // ============================================================
            // RELACIONES CON PLAYERS (Doble FK a la misma tabla)
            // ============================================================

            // Configuración Jugador 1
            builder.HasOne(c => c.Player1)
                .WithMany() // No necesitamos una lista de "Parejas" en la entidad Player para no ensuciar
                .HasForeignKey(c => c.Player1Id)
                .OnDelete(DeleteBehavior.Restrict); // No borrar jugadores si se borra la pareja

            // Configuración Jugador 2
            builder.HasOne(c => c.Player2)
                .WithMany()
                .HasForeignKey(c => c.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}