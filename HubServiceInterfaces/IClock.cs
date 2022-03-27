namespace HubServiceInterfaces;

public interface IClock
{
    Task ShowTime(DateTime currentTime);
}

public interface IPublishCPU
{
    Task Publish(CpuUsage cpuUsage);
}