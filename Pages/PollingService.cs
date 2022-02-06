using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MyBlazorServerApp.Pages
{
    public class PollingService
    {
        private Timer timer;
        public ProductDetails[] Cache;

        public static PollingService Instance = new PollingService();

        public PollingService()
        {
            timer = new Timer(Refresh, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Cache = new ProductDetails[200_000];
        }

        private void Refresh(object? state)
        {
            if(_paused)
                return;
            for (var i = 0; i < Cache.Length; i++)
            {
                Cache[i] = ProductDetails.BuildOne(i);
            }

            //Debug.WriteLine($"handlers count = {_handlers.Count}");
            foreach (var handler in _handlers.Values)
            {
                handler();
            }
        }

        private Dictionary<int, Action> _handlers = new Dictionary<int, Action>();
        private bool _paused;

        public void Register(int subscriber, Action refreshData)
        {
            _handlers.Add(subscriber, refreshData);
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }

        public void UnRegisterAll()
        {
            _handlers.Clear();
        }
    }
}