using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SuwoGym_API.Services;
using System.Security.Claims;
using System.Text;

var builder =
    WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

/*
 * Permite que UtilesService lea los claims
 * del usuario autenticado.
 */
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<
    IUtilesService,
    UtilesService>();

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,

                ValidateAudience = false,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration[
                                "Jwt:SecretKey"]!)),

                RoleClaimType =
                    ClaimTypes.Role,

                ClockSkew =
                    TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

var app =
    builder.Build();

app.UseExceptionHandler(
    "/api/Error/RegistrarError");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();