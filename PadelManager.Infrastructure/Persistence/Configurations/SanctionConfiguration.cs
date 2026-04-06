using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class SanctionConfiguration : IEntityTypeConfiguration<Sanction>
    {
        public void Configure(EntityTypeBuilder<Sanction> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Reason)
                .IsRequired()
                .HasMaxLength(700);

            builder.Property(s => s.ExpirationDate)
                .IsRequired();

            builder.Property(s => s.Severity)
                .HasConversion<string>()
                .IsRequired();


            // Relación con el jugador
            builder.HasOne(s => s.Player)
                .WithMany(p => p.Sanctions)
                .HasForeignKey(s => s.PlayerId);
        }
    }
}
