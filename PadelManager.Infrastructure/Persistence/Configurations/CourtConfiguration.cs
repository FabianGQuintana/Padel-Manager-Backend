using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class CourtConfiguration : IEntityTypeConfiguration<Court>
    {
        public void Configure(EntityTypeBuilder<Court> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CourtAvailability)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.SurfaceType)
                .IsRequired()
                .HasMaxLength(100);

            //Fks

            builder.HasOne(c => c.Venue)
                .WithMany(v => v.Courts)
                .HasForeignKey(c => c.VenueId);
        }
    }
}
