using F1Simulator.CompetitionService.Data;
using F1Simulator.CompetitionService.Repositories;
using F1Simulator.CompetitionService.Repositories.Interfaces;
using F1Simulator.CompetitionService.Services;
using F1Simulator.CompetitionService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<CompetitionServiceConnection>();
builder.Services.AddScoped< ICircuitRepository, CircuitRepository>();
builder.Services.AddScoped< ICircuitService, CircuitService>();

builder.Services.AddScoped<ICompetitionRepository, CompetitionRepository>();
builder.Services.AddScoped<ICompetitionService, CompetitionService>();

builder.Services.AddHttpClient("GetTeamsClient", client => client.BaseAddress = new Uri(""));
builder.Services.AddHttpClient("GetDriversClient", client => client.BaseAddress = new Uri(""));
builder.Services.AddHttpClient("GetCountTeams", cliente => cliente.BaseAddress = new Uri(""));
builder.Services.AddHttpClient("GetCountDriversClient", cliente => cliente.BaseAddress = new Uri(""));
builder.Services.AddHttpClient("GetCountCarsClient", cliente => cliente.BaseAddress = new Uri(""));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
