
using F1Simulator.CompetitionService.Repositories;
using F1Simulator.CompetitionService.Repositories.Interfaces;
using F1Simulator.CompetitionService.Services;
using F1Simulator.CompetitionService.Services.Interfaces;
using F1Simulator.Utils.DatabaseConnectionFactory;
using F1Simulator.Utils.DatabaseConnectionFactory.Connections;
using Microsoft.Data.SqlClient;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IDatabaseConnection<SqlConnection>, SqlServerConnection>();
builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory { HostName = "localhost" });
builder.Services.AddScoped< ICircuitRepository, CircuitRepository>();
builder.Services.AddScoped< ICircuitService, CircuitService>();

builder.Services.AddScoped<ICompetitionRepository, CompetitionRepository>();
builder.Services.AddScoped<ICompetitionService, CompetitionService>();

builder.Services.AddHttpClient("GetTeamsClient", client => client.BaseAddress = new Uri("https://localhost:8001/api/team/"));
builder.Services.AddHttpClient("GetDriversClient", client => client.BaseAddress = new Uri("https://localhost:8001/api/driver/"));
builder.Services.AddHttpClient("GetCountCarsClient", cliente => cliente.BaseAddress = new Uri("https://localhost:8001/api/car"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
