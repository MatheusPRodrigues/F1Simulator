using F1Simulator.RaceControlService.Messaging;
using F1Simulator.RaceControlService.Repositories;
using F1Simulator.RaceControlService.Repositories.Interfaces;
using F1Simulator.RaceControlService.Services;
using F1Simulator.RaceControlService.Services.Interfaces;
using F1Simulator.Utils.DatabaseConnectionFactory;
using F1Simulator.Utils.DatabaseConnectionFactory.Config;
using F1Simulator.Utils.DatabaseConnectionFactory.Connections;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

BsonSerializer.RegisterSerializer(
    new GuidSerializer(GuidRepresentation.Standard)
);

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<MongoDBSettings>();

builder.Services.AddSingleton<IDatabaseConnection<IMongoDatabase>, MongoConnection>();

builder.Services.AddScoped<IRaceControlService, RaceControlService>();
builder.Services.AddScoped<IRaceControlRepository, RaceControlRepository>();
builder.Services.AddSingleton<IPublishService, RabbitMQPublish>();

builder.Services.AddHttpClient("EngineeringService", client =>
{
    client.BaseAddress = new Uri("https://localhost:6001/api/engineering/");
});

builder.Services.AddHttpClient("TeamManagementServicesDrivers", client =>
{
    client.BaseAddress = new Uri("https://localhost:8001/api/");
});

builder.Services.AddHttpClient("CompetitionService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/api/competition/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
