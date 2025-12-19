using F1Simulator.RaceControlService.Messaging;
using F1Simulator.RaceControlService.Repositories;
using F1Simulator.RaceControlService.Repositories.Interfaces;
using F1Simulator.RaceControlService.Services;
using F1Simulator.RaceControlService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IRaceControlService, RaceControlService>();
builder.Services.AddScoped<IRaceControlRepository, RaceControlRepository>();
builder.Services.AddSingleton<IPublishService, RabbitMQPublish>();

builder.Services.AddHttpClient("EngineeringService", client =>
{
    client.BaseAddress = new Uri("https://localhost:6001/api/engineering/");
});

builder.Services.AddHttpClient("TeamManagementServicesDrivers", client =>
{
    client.BaseAddress = new Uri("https://localhost:8001/api");
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
