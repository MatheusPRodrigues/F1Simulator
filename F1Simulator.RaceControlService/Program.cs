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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
