using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SuwoGym_Web.Deployment;

var builder = WebApplication.CreateBuilder(args);

// La persistencia evita pérdida de cookies al reiniciar la instancia.
// En despliegue se requiere un volumen privado externo a wwwroot.
if (!builder.Environment.IsDevelopment())
{
    ConfiguracionPreproduccionWeb.Validar(builder.Configuration);
    builder.Services.AddDataProtection()
        .SetApplicationName("SuwoGym.Web")
        .PersistKeysToFileSystem(new DirectoryInfo(
            builder.Configuration["Deployment:DataProtectionKeysPath"]!));
}

// Add services to the container.
builder.Services.AddControllersWithViews();

// Antifalsificación de formularios: cookie protegida en HTTPS.
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Agregar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Agregar HttpClientFactory
builder.Services.AddHttpClient();

// No expone diagnóstico ni conecta a cuentas de usuarios.
builder.Services.AddHealthChecks()
    .AddCheck<ApiReadinessCheck>("api");

// Agregar HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error/CapturarError");
app.UseHsts();

app.UseHttpsRedirection();
app.UseRouting();

// Usar sesiones
app.UseSession();

// Validar en la API la versión de sesión vigente antes de cada ruta privada.
app.UseMiddleware<SuwoGym_Web.Middleware.ValidarSesionJwtMiddleware>();

app.UseAuthorization();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});
app.MapHealthChecks("/health/ready");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HomePage}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
