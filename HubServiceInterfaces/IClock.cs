using Microsoft.AspNetCore.SignalR;

namespace HubServiceInterfaces;

public interface IClock
{
    Task ShowTime(DateTime currentTime);
}

public class ClockHub : Hub<IClock>
{
    public async Task SendTimeToClients(DateTime dateTime)
    {
        await Clients.All.ShowTime(dateTime);
    }
}

public interface IPublishCPU
{
    Task Publish(CpuUsage cpuUsage);
}

public class CpuUsageHub : Hub<IPublishCPU>
{
    public async Task Publish(CpuUsage cpuUsage)
    {
        await Clients.All.Publish(cpuUsage);
    }
}