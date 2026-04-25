using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // 1. Clave Primaria
            builder.HasKey(r => r.Id);

            // 2. Configuración de NameRol (La clave de nuestra charla anterior)
            builder.Property(r => r.NameRol)
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion<string>(); // Convierte el Enum a String en la DB

            builder.HasMany(r => r.Users)
                   .WithOne(u => u.Role)
                   .HasForeignKey(u => u.RoleId)
                   .OnDelete(DeleteBehavior.Restrict);
            // Usamos Restrict para que no se pueda borrar un Rol si tiene usuarios asignados.
        }
    }
}