using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PadelManager.Application.Interfaces.Common;

namespace PadelManager.Infrastructure.Persistence.Factories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PadelManagerDbContext>
    {
        public PadelManagerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PadelManagerDbContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PadelManagerDB;Username=postgres;Password=maxiximo21");

          
            var designTimeUser = new DesignTimeCurrentUser();

            return new PadelManagerDbContext(optionsBuilder.Options, designTimeUser);
        }
    }

    // Clase auxiliar simple para que EF no se queje por el constructor
    public class DesignTimeCurrentUser : ICurrentUser
    {
        public string? UserId => "System_Migration";
        public string? UserName => "Migration_User";
        public Guid Id => Guid.Empty;
        public bool IsAuthenticated => false;
    }
}