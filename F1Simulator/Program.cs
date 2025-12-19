using F1Simulator.TeamManagementService.Data;
using F1Simulator.TeamManagementService.Repositories;
using F1Simulator.TeamManagementService.Repositories.Interfaces;
using F1Simulator.TeamManagementService.Services;
using F1Simulator.TeamManagementService.Services.Interfaces;
using F1Simulator.Utils.Clients;
using F1Simulator.Utils.Clients.Interfaces;
using F1Simulator.Utils.DatabaseConnectionFactory;
using F1Simulator.Utils.DatabaseConnectionFactory.Connections;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter()
                    );
                });

builder.Services.AddSingleton<IDatabaseConnection<SqlConnection>, SqlServerConnection>();

builder.Services.AddSingleton<IEngineerRepository, EngineerRepository>();
builder.Services.AddSingleton<IEngineerService, EngineerService>();

builder.Services.AddSingleton<ITeamRepository, TeamRepository>();
builder.Services.AddSingleton<ITeamService, TeamService>();

builder.Services.AddSingleton<IBossRepository, BossRepository>();
builder.Services.AddSingleton<IBossService, BossService>();

builder.Services.AddSingleton<IDriverRepository, DriverRepository>();
builder.Services.AddSingleton<IDriverService, DriverService>();

builder.Services.AddSingleton<ICarRepository, CarRepository>();
builder.Services.AddSingleton<ICarService, CarService>();

builder.Services.AddHttpClient("TeamManagement", client =>
{
    client.BaseAddress = new Uri("https://localhost:8001/api/");
});


builder.Services.AddHttpClient<ICompetitionClient, CompetitionClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
