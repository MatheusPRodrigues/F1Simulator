using F1Simulator.EngineeringService.Services;
using F1Simulator.EngineeringService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IEngineeringService, EngineeringService>();

builder.Services.AddHttpClient("Car", client => client.BaseAddress = new Uri("https://localhost:8001/api/"));

builder.Services.AddHttpClient("Engineer", client => client.BaseAddress = new Uri("https://localhost:8001/api/"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
