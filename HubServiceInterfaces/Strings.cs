namespace HubServiceInterfaces;

public static class Strings
{
    public static string HubUrl => "https://localhost:57576/hubs/cpu";

    public static class Events
    {
        public static string TimeSent => nameof(IClock.ShowTime);
        public static string PublishCPU => nameof(IPublishCPU.Publish);
    }
}

public class CpuUsage
{
    public DateTime Time { get; set; }
    public double CPU_Usage { get; set; }
}
