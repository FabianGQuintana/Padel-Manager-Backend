using Microsoft.EntityFrameworkCore;
using PadelManager.Application.Interfaces.Common;
using PadelManager.Application.Interfaces.Persistence;
using PadelManager.Application.Interfaces.Repositories;
using PadelManager.Application.Interfaces.Services;
using PadelManager.Infrastructure.Persistence;
using PadelManager.Infrastructure.Repositories;
using PadelManager.Infrastructure.Services;
using PadelManager.Application.Services;
using Microsoft.AspNetCore.Identity;
using PadelManager.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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

builder.Services.AddHttpContextAccessor();

// --- Espacio para que el equipo agregue sus repositorios específicos ---
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IStatisticRepository, StatisticRepository>();
builder.Services.AddScoped<ICoupleRepository, CoupleRepository>();
builder.Services.AddScoped<IStageRepository, StageRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
builder.Services.AddScoped<IMatchRepository,MatchRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICoupleAvailabilityRepository, CoupleAvailabilityRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<ISanctionRepository, SanctionRepository>();
builder.Services.AddScoped<ITournamentFinanceRepository, TournamentFinanceRepository>();
builder.Services.AddScoped<ICourtRepository, CourtRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
#endregion



#region INYECCIÓN DE DEPENDENCIAS - SERVICIOS (Lógica de Negocio)
// =========================================================================
// 4. INYECCIÓN DE DEPENDENCIAS - SERVICIOS (Lógica de Negocio)
// =========================================================================
// Aquí irán los Services que consumirán los repositorios
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IZoneService, ZoneService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<IStageService, StageService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMatchService, MatchService>();    
builder.Services.AddScoped<ICoupleService, CoupleService>();    
builder.Services.AddScoped<ICoupleService, CoupleService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<ICoupleAvailabilityService, CoupleAvailabilityService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IManagerService, ManagerService>();

#endregion



#region Infraestructura / Seguridad

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
#endregion



#region JWT Authorize

builder.Services.AddAuthentication(options =>
{
    
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

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

#region PIPELINE DE LA APP.
// =========================================================================
// 7. PIPELINE DE LA APLICACIÓN (ORDEN CRÍTICO)
// =========================================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Esto habilita Swagger/OpenAPI
}

app.UseHttpsRedirection();

// 1. CORS: Primero dejamos entrar la petición
app.UseCors("PadelFrontendPolicy");

// 2. AUTHENTICATION: ¿Quién sos? (Lee el Token)
app.UseAuthentication();

// 3. AUTHORIZATION: ¿Tenés permiso para esto? (Mira los Roles)
app.UseAuthorization();

// 4. ROUTING: Mandamos la petición al controlador
app.MapControllers();

app.Run();
#endregion