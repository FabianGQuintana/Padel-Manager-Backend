using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            //Naming the table
            builder.ToTable("Managers");

            //Primary Key
            builder.HasKey(m => m.Id);

            builder.HasQueryFilter(t => t.DeletedAt == null);

            //Properties configuration
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(m => m.LastName)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(m => m.Dni)
                .IsRequired();
            builder.HasIndex(m => m.Dni)
                .IsUnique();

            builder.Property(m => m.PhoneNumber)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(m => m.Email)
                .IsRequired();
            builder.HasIndex(m => m.Email)
                .IsUnique();

        }
    }
}
