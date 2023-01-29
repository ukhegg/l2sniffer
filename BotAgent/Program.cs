using BotAgent;
using SniffingService = BotAgent.GrpcServices.SniffingService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
DI.SetupServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGrpcService<SniffingService>();
app.MapGet(
    "/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run("http://localhost:13642");