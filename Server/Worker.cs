using System.Diagnostics;
using HubServiceInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace Server;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHubContext<ClockHub, IClock> _clockHub;
    private readonly IHubContext<CpuUsageHub, IPublishCPU> _cpuHub;
    private readonly Random _random = new();
    private int _dataLength = 100;

    public Worker(ILogger<Worker> logger, IHubContext<ClockHub, IClock> clockHub, IHubContext<CpuUsageHub, IPublishCPU> cpuHub)
    {
        _logger = logger;
        _clockHub = clockHub;
        _cpuHub = cpuHub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dataLength = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {Time}", DateTime.Now);
            
            // await _clockHub.Clients.All.ShowTime(DateTime.Now);
            _dataLength++;
            await _cpuHub.Clients.All.Publish(new CpuUsage
            {
                Time = DateTime.Now,//.AddSeconds(dataLength + 10)
                CPU_Usage = _random.Next(30, 80)
            });

            await Task.Delay(1000, stoppingToken);
        }
    }
}