using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class CoupleAvailabilityConfiguration : IEntityTypeConfiguration<CoupleAvailability>
    {
        public void Configure(EntityTypeBuilder<CoupleAvailability> builder)
        {
            builder.ToTable("CoupleAvailabilities");

            builder.HasKey(ca => ca.Id);

            builder.Property(ca => ca.Day)
                .IsRequired();

            builder.Property(ca => ca.From)
                .IsRequired();

            builder.Property(ca => ca.To)
                .IsRequired();

            builder.HasOne(ca => ca.Couple)
                .WithMany(c => c.Availabilities)
                .HasForeignKey(ca => ca.CoupleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ca => new { ca.CoupleId, ca.Day });

            builder.HasQueryFilter(ca => ca.DeletedAt == null);
        }
    }
}