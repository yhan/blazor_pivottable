using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNetCore.Connections.Features;
using Syncfusion.Blazor.Internal;

namespace MyBlazorServerApp.Pages
{
    public class PollingService
    {
        private Timer _timer;
        public ObservableCollection<ProductDetails> Observable { get; set; }
        private readonly object _syncRoot = new object();
        private const int InitSize = 200_000;
        private const int TimerIntervalSecond = 5;

        private static PollingService _instance;

        private readonly Random _rand = new Random();
        private readonly Dictionary<int, Action> _handlers = new Dictionary<int, Action>();
        private bool _paused;


        public static PollingService Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new PollingService();
                }

                return _instance;
            }
        }


        public PollingService()
        {
            //var arr = new ProductDetails[InitSize];
            Observable = new ObservableCollection<ProductDetails>(/*arr*/);
            _timer = new Timer(Refresh, null, TimeSpan.Zero, TimeSpan.FromSeconds(TimerIntervalSecond));
        }

        private void Refresh(object? state)
        {
            if (_paused)
                return;

            var count = _rand.Next((int)(InitSize * 0.8), (int)(InitSize * 1.2));
            var arr = new ProductDetails[count];
            for (var j = 0; j < count; j++)
            {
                arr[j] = ProductDetails.BuildOne(j);
            }
            
            lock (_syncRoot)
            {
                Observable.Clear();
                foreach (var a in arr)
                {
                    Observable.Add(a);
                }
                //Debug.WriteLine($"handlers count = {_handlers.Count}");
                foreach (var handler in _handlers.Values)
                {
                    handler();
                }
            }
        }

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