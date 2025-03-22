using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.UTF8.GetBytes("45262B37D9B63986B437DEBD5C8EA45262B37D9B63986B437DEBD5C8EA");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://auth:8082"; // URL do serviço de autenticação
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Temporariamente desativado
            ValidateAudience = false, // Temporariamente desativado
            ValidateLifetime = false, // Temporariamente desativado
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://auth:8082", // URL do serviço de autenticação no Docker
            ValidAudience = "http://gatewayapi:8080", // URL do serviço de gateway no Docker
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
