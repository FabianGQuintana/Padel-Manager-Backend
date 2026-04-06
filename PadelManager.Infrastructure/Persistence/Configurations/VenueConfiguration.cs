using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.City)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(v => v.Address)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(v => v.PhoneNumber)
                .IsRequired(false)
                .HasMaxLength(30);
        }
    }
}
