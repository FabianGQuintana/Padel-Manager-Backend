using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PadelManager.Domain.Entities;
using PadelManager.Application.Interfaces.Common; 

namespace PadelManager.Infrastructure.Persistence
{
    public class PadelManagerDbContext : DbContext
    {
        private readonly ICurrentUser _currentUser;

        // Ahora el constructor recibe también nuestro servicio de usuario actual
        public PadelManagerDbContext(
            DbContextOptions<PadelManagerDbContext> options,
            ICurrentUser currentUser) : base(options)
        {
            _currentUser = currentUser;
        }

        #region DbSets (Tablas)
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Couple> Couples { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoupleAvailability> CoupleAvailabilities { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Sanction> Sanctions { get; set; }
        public DbSet<TournamentFinance> TournamentFinances { get; set; }
        public DbSet<Payment> Payments { get; set; }
        #endregion



        //Metodo que dispara cuando el sistema se inicia para dar las reglas de cada entidad.
        //Buscara en el proyecto las clases de configuracion de cada entidad y las aplicara.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        // ==========================================
        // LÓGICA DE AUDITORÍA AUTOMÁTICA (no estoy seguro)
        // ==========================================
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Interceptamos todas las entidades que hereden de BaseEntity
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                var now = DateTime.UtcNow;
                var user = _currentUser.UserId ?? "System";

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = user;
                        entry.Entity.Status = "Active"; // Estado inicial por defecto
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = now;
                        entry.Entity.LastModifiedBy = user;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}