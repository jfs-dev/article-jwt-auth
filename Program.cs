using article_jwt_auth.Models;
using article_jwt_auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes(AppSettingsService.JwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapPost("/v1/authenticate/", [AllowAnonymous] (User user) =>
    {
        if (user.Email == "e-mail@dominio.com.br" && user.Password == "SenhaForte")
            return Results.Ok(JwtBearerService.GenerateToken(user));
        
        return Results.Unauthorized();
    });

app.MapGet("/v1/anonymous/", [AllowAnonymous] () => "Anônimo");

app.MapGet("/v1/user/", () => "Usuário").RequireAuthorization();

app.MapGet("/v1/admin/", [Authorize(Roles = "Admin")]() => "Administrador").RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
