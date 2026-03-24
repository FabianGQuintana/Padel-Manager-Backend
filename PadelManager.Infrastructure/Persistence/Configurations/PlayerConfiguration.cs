using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Players");

            builder.HasKey(p => p.Id);

            builder.HasQueryFilter(t => t.DeletedAt == null);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(p => p.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
            builder.HasIndex(p => p.PhoneNumber)
                .IsUnique();

            builder.Property(p => p.Dni)
                .IsRequired()
                .HasMaxLength(8); // Argentina's DNI has 8 digits
            builder.HasIndex(p => p.Dni)
                .IsUnique();

            builder.Property(p => p.Age)
                .IsRequired(false);




        }
    }
}
