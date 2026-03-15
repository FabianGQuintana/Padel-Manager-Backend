using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Infrastructure.Persistence;
using PadelManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

#region CONFIGURACIÓN DE CONTROLADORES Y OPENAPI
// =========================================================================
// 1. CONFIGURACIÓN DE CONTROLADORES Y OPENAPI
// =========================================================================
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // OpenAPI / Swagger

#endregion


#region  CONEXIÓN A BASE DE DATOS (PostgreSQL)
// =========================================================================
// 2. CONEXIÓN A BASE DE DATOS (PostgreSQL)
// =========================================================================
builder.Services.AddDbContext<PadelManagerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion



#region INYECCIÓN DE DEPENDENCIAS - REPOSITORIOS Y UNIT OF WORK
// =========================================================================
// 3. INYECCIÓN DE DEPENDENCIAS - REPOSITORIOS Y UNIT OF WORK
// =========================================================================
// Es vital registrar el UnitOfWork primero
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// --- Espacio para que el equipo agregue sus repositorios específicos ---
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IStatisticRepository, StatisticRepository>();
builder.Services.AddScoped<ICoupleRepository, CoupleRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IStageRepository, StageRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
builder.Services.AddScoped<IMatchRepository,MatchRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


#endregion



#region INYECCIÓN DE DEPENDENCIAS - SERVICIOS (Lógica de Negocio)
// =========================================================================
// 4. INYECCIÓN DE DEPENDENCIAS - SERVICIOS (Lógica de Negocio)
// =========================================================================
// Aquí irán los Services que consumirán los repositorios
// builder.Services.AddScoped<ITournamentService, TournamentService>();

#endregion



#region CONFIGURACIÓN DE SEGURIDAD Y CORS (Acceso desde el Frontend)
// =========================================================================
// 5. CONFIGURACIÓN DE SEGURIDAD Y CORS (Acceso desde el Frontend)
// =========================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("PadelFrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Puerto típico de Vite/React
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

#endregion


#region CONSTRUCCIÓN DE LA APP
// =========================================================================
// 6. CONSTRUCCIÓN DE LA APP
// =========================================================================
var app = builder.Build();

#endregion


#region PIPELINE DE LA APLICACIÓN (MIDDLEWARES)
// =========================================================================
// 7. PIPELINE DE LA APLICACIÓN (MIDDLEWARES)
// =========================================================================

// Configuración para el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Importante: El UseCors debe ir DESPUÉS de HttpsRedirection y ANTES de Authorization
app.UseCors("PadelFrontendPolicy");

app.UseAuthorization();

app.MapControllers();


#endregion

#region LANZAMIENTO
// =========================================================================
// 8. LANZAMIENTO
// =========================================================================
app.Run();

#endregion
