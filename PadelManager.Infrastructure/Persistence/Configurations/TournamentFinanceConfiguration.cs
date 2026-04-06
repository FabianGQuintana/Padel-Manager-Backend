using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class TournamentFinanceConfiguration : IEntityTypeConfiguration<TournamentFinance>
    {
        public void Configure(EntityTypeBuilder<TournamentFinance> builder)
        {
            builder.HasKey(tf => tf.Id);

            builder.Property(tf => tf.FinanceConcept)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(tf => tf.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(tf => tf.MovementType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(tf => tf.DateMovement)
                .IsRequired();

            // Relación con el torneo
            builder.HasOne(tf => tf.Tournament)
                .WithMany()
                .HasForeignKey(tf => tf.TournamentId);
        }
    }
}
