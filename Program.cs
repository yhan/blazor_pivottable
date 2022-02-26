using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace blazor_pivottable
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTc3NTQ1QDMxMzkyZTM0MmUzMGZIekFaNjBaTk5NK1RtMFBFNk1od2dMTVJDMmdhQWRjaHN6SWdETEErVXc9");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
