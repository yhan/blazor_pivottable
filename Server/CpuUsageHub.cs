using HubServiceInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace Server;

public class CpuUsageHub : Hub<IPublishCPU>
{
    public async Task Publish(CpuUsage cpuUsage)
    {
        await Clients.All.Publish(cpuUsage);
    }
}