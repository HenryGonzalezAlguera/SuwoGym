using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using SuwoGym_API.Services;
using System.Security.Claims;
using System.Text;

var builder =
    WebApplication.CreateBuilder(args);

// La clave de firma nunca debe almacenarse en appsettings versionados.
// En desarrollo, configúrela con dotnet user-secrets o variables de entorno.
var jwtSecret = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrWhiteSpace(jwtSecret) ||
    Encoding.UTF8.GetByteCount(jwtSecret) < 32)
{
    throw new InvalidOperationException(
        "Falta Jwt:SecretKey o tiene menos de 32 bytes. " +
        "Configure un valor nuevo mediante .NET User Secrets " +
        "o una variable de entorno antes de iniciar la API.");
}


var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
if (string.IsNullOrWhiteSpace(jwtIssuer) ||
    string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException(
        "Faltan Jwt:Issuer o Jwt:Audience. " +
        "Revise la configuración de autenticación de la API.");
}

// En Staging y Production, impedir iniciar con localhost, TLS débil
// o credenciales ausentes. Development mantiene su configuración actual.
if (!builder.Environment.IsDevelopment())
    ConfiguracionPreproduccionApi.Validar(builder.Configuration);

builder.Services.AddControllers();

/*
 * Permite que UtilesService lea los claims
 * del usuario autenticado.
 */
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<
    IUtilesService,
    UtilesService>();

// La versión se consulta en SQL para cada token, sin cachear permisos antiguos.
builder.Services.AddScoped<ISeguridadJwtVersionService, SeguridadJwtVersionService>();

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSecret)),

                RoleClaimType =
                    ClaimTypes.Role,

                ClockSkew =
                    TimeSpan.Zero
            };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var identity = context.Principal;
                if (!int.TryParse(identity?.FindFirst("IdUsuario")?.Value, out var id)
                    || !int.TryParse(identity?.FindFirst("IdRol")?.Value, out var rol)
                    || !Guid.TryParse(identity?.FindFirst("VersionJwt")?.Value, out var version)
                    || identity?.IsInRole(rol.ToString()) != true)
                {
                    context.Fail("JWT sin datos de sesión válidos.");
                    return;
                }

                try
                {
                    var seguridad = context.HttpContext.RequestServices
                        .GetRequiredService<ISeguridadJwtVersionService>();
                    var actual = await seguridad.ConsultarAsync(
                        id, context.HttpContext.RequestAborted);
                    if (actual?.PermiteToken(version, rol) != true)
                    {
                        context.Fail("La sesión dejó de ser válida.");
                    }
                }
                catch (Exception)
                {
                    // Fallar cerrado ante pérdida de conexión, migración ausente,
                    // timeout o cualquier error de SQL. No mostrar excepciones al cliente.
                    context.Fail("No se pudo validar la sesión.");
                }
            }
        };
    });

// Control de abuso en endpoints públicos: partición por IP del cliente.
// En despliegues con proxy se debe configurar ForwardedHeaders y proxies de confianza.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("auth-login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 12,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            }));

    // Verificación de enlace y consumo de token; independiente del correo inicial.
    options.AddPolicy("auth-reset", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(15),
                QueueLimit = 0,
                AutoReplenishment = true
            }));

    options.AddPolicy("auth-recovery", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 4,
                Window = TimeSpan.FromMinutes(15),
                QueueLimit = 0,
                AutoReplenishment = true
            }));
});

builder.Services.AddAuthorization();

// Liveness no toca SQL; readiness comprueba SQL + objetos críticos.
builder.Services.AddHealthChecks()
    .AddCheck<SqlReadinessCheck>("sql");

var app =
    builder.Build();

app.UseExceptionHandler(
    "/api/Error/RegistrarError");

app.UseHttpsRedirection();

app.UseRouting();
app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});
app.MapHealthChecks("/health/ready");

app.Run();

