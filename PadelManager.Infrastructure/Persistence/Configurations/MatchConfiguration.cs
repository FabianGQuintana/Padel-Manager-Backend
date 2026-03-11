using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelManager.Domain.Entities;
using System;


namespace PadelManager.Infrastructure.Persistence.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>   
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            //Naming the table
            builder.ToTable("Matches");

            //Primary Key
            builder.HasKey(m => m.Id);

            //Properties configuration

            builder.Property(m => m.Loser)
                .IsRequired();

            builder.Property(m => m.DateTime)
                .IsRequired();

            builder.Property(m => m.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(m => m.LocationName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.CourtName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Set1_coupleA)
                .IsRequired();

            builder.Property(m => m.Set1_coupleB)
                .IsRequired();

            builder.Property(m => m.Set2_coupleA)
                .IsRequired();

            builder.Property(m => m.Set2_coupleB)
                .IsRequired();

            //SETS condicionales
            builder.Property(m => m.Set3_coupleA)
                .IsRequired(false);


            builder.Property(m => m.Set3_coupleB)
                .IsRequired(false);

            //Foreign Keys configuration

            // 1. Relación con Instance (1:N)
            // Una Instancia tiene muchos Partidos, un Partido tiene una Instancia.
            builder.HasOne(m => m.Instance)
                   .WithMany(i => i.Matches)
                   .HasForeignKey(m => m.InstanceId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 2. Relación con la Pareja A (Primer "Padre")
            builder.HasOne(m => m.Couple) 
                   .WithMany() 
                   .HasForeignKey(m => m.CoupleId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 3. Relación con la Pareja B (Segundo "Padre")
            builder.HasOne(m => m.Couple2)
                   .WithMany()
                   .HasForeignKey(m => m.CoupleId2)
                   .OnDelete(DeleteBehavior.Restrict);
            //No ponemos una relacion de N:M porque si no, en un partido pueden haber 50
            //parejas por ejemplo.
            // En este caso, cada partido tiene exactamente dos parejas,
            // y cada pareja puede participar en muchos partidos,
            // pero no queremos que una pareja pueda ser eliminada si está
            // asociada a un partido, por eso usamos Restrict.
        }
    }
}
