using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);

        builder.Property(u => u.Dni).IsRequired().HasMaxLength(20);
        builder.HasIndex(u => u.Dni).IsUnique();

        builder.Property(u => u.PhoneNumber).HasMaxLength(20);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash).IsRequired();

        // Relación 1:1 con Manager
        builder.HasOne(u => u.Manager)
               .WithOne(m => m.User)
               .HasForeignKey<Manager>(m => m.UserId)
               .OnDelete(DeleteBehavior.Cascade); // Si se borra el User, vuela el Manager

        // Relación 1:N con Role
        builder.HasOne(u => u.Role)
               .WithMany(r => r.Users)
               .HasForeignKey(u => u.RoleId);
    }
}