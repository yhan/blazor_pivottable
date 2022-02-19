using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBlazorServerApp.Data;
using MyBlazorServerApp.Pages;
using Syncfusion.Blazor;

namespace MyBlazorServerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<PollingService>();

            services.AddSignalR(e =>
            {
                // avoid the server disconnection problem
                e.MaximumReceiveMessageSize = 102_400_000; // 100 MB
            });

            services.AddSyncfusionBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Your Syncfusion License Key");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }

    public class PausableTimer
    {
        public event EventHandler Elapsed;
        private Timer mTimer;
        private bool mEnabled;
        private int mResidue;
        private DateTime mStart;
        private object mLocker;

        public PausableTimer()
        {
            mTimer = new Timer(callback);
            mLocker = new object();
        }

        public int Interval { get; set; }

        public bool Enabled
        {
            get { return mEnabled; }
            set
            {
                lock (mLocker)
                {
                    if (value == mEnabled) return;
                    mEnabled = value;
                    if (value)
                    {
                        mStart = DateTime.Now;
                        mTimer.Change(Interval - mResidue, Interval);
                    }
                    else
                    {
                        mTimer.Change(0, 0);
                        mResidue = Math.Min(Interval, (int)(DateTime.Now - mStart).TotalMilliseconds);
                    }
                }
            }
        }

        public void Reset()
        {
            lock (mLocker)
            {
                Enabled = false;
                mResidue = 0;
            }
        }

        private void callback(object dummy)
        {
            bool fire;
            lock (mLocker)
            {
                mStart = DateTime.Now;
                fire = Elapsed != null && mEnabled;
            }
            try
            {  // System.Timers.Timer.Elapsed swallows exceptions, bah
                if (fire) Elapsed(this, EventArgs.Empty);
            }
            catch { }
        }
    }
}
