using Cadastro.Auth.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços da infraestrutura (inclui TokenService e DbContext)
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();
var secret = builder.Configuration.GetValue<string>("Secret");

var key = Encoding.ASCII.GetBytes(secret);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}



app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
