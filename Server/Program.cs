using HubServiceInterfaces;
using Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapHub<ClockHub>("/hubs/clock");
app.MapHub<CpuUsageHub>("/hubs/cpu");

app.Run();