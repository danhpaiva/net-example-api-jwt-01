using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using UsuarioApi.Models;
using UsuarioApi.Services;

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingsService.JwtSettings.Key)),
        ValidateIssuer = false,
        ValidateAudience = false
      };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/v1/authenticate/", [AllowAnonymous] (Usuario usuario) =>
    {
      if (usuario.Email == "e-mail@dominio.com.br" && usuario.Password == "SenhaForte")
        return Results.Ok(JwtBearerService.GenerateToken(usuario));

      return Results.Unauthorized();
    });

app.MapGet("/v1/anonymous/", [AllowAnonymous] () => "Anônimo");

app.MapGet("/v1/user/", () => "Usuário").RequireAuthorization();

app.MapGet("/v1/admin/", [Authorize(Roles = "Admin")] () => "Administrador").RequireAuthorization();

app.Run();
