using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Prometheus;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adicionando a configuração do Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

builder.Services.AddControllers();

var key = Encoding.UTF8.GetBytes("45262B37D9B63986B437DEBD5C8EA45262B37D9B63986B437DEBD5C8EA");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://auth:8082"; // URL do serviço Auth
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://auth:8082", // Emissor do token
            ValidAudience = "http://gatewayapi:8080", // Público do token
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gateway API",
        Version = "v1",
        Description = "API para Gateway",
    });
});

var app = builder.Build();

// Registro do middleware de Rate Limiting
app.UseMiddleware<RateLimitingMiddleware>();

app.UseMetricServer();
app.UseHttpMetrics();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API V1");
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapMetrics();

app.UseOcelot().Wait();

app.Run();
