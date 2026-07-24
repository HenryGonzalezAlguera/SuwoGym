using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SuwoGym_API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IUtilesService, UtilesService>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
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

                ClockSkew = TimeSpan.Zero
            };
    });

var app = builder.Build();

app.UseExceptionHandler(
    "/api/Error/RegistrarError");

app.UseHttpsRedirection();

/*
 * El orden es obligatorio:
 * primero autenticación y después autorización.
 */
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();