using System;
using Microsoft.EntityFrameworkCore;
using PadelManager.Domain.Entities;
using System.Reflection;

namespace PadelManager.Infrastructure.Persistence
{
    public class PadelManagerDbContext : DbContext
    {
        //Constructor que recibe la connection string de appsettings.json y
        //la pasa a la clase base DbContext para establecer la conexion con la base de datos.
        public PadelManagerDbContext(DbContextOptions<PadelManagerDbContext> options) : base(options)
        
        {
            //Base(options) = Ejecutá primero el constructor de mi madre(DbContext) usando esta
            //configuración (las credenciales de pgAdmin4) para que ella prepare el
            //terreno antes de que yo haga mis cosas
        }

        //Tablas para cada entidad
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

        //Metodo que dispara cuando el sistema se inicia para dar las reglas de cada entidad.
        //Buscara en el proyecto las clases de configuracion de cada entidad y las aplicara.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  
        }
    }
}
