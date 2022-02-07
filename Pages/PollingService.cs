using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Syncfusion.Blazor.Internal;

namespace MyBlazorServerApp.Pages
{
    public class PollingService
    {
        private Timer _timer;
        public ObservableCollection<ProductDetails> Observable { get; set; }
        private int _maxId = 0;
        private readonly object _syncRoot = new object();
        private const int Size = 200_000;
        private const int TimerIntervalSecond = 10;

        private static PollingService _instance;


        public static PollingService Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new PollingService(new ProductDetails[Size]);
                }

                return _instance;
            }
        }

        private readonly Dictionary<int, Action> _handlers = new Dictionary<int, Action>();
        private bool _paused;


        public PollingService(ProductDetails[] arr)
        {
            Observable = new ObservableCollection<ProductDetails>(arr);
            _timer = new Timer(Refresh, null, TimeSpan.Zero, TimeSpan.FromSeconds(TimerIntervalSecond));
            //Refresh(null);
        }

        private void Refresh(object? state)
        {
            lock (_syncRoot)
            {
                if (_paused)
                    return;

                for (var i = 0; i < Observable.Count; i++)
                {
                    if (Observable[i] == null)
                    {
                        Observable[i] = ProductDetails.BuildOne(i);
                    }
                    else
                    {
                        Observable[i].Random();
                    }
                }

                _maxId = Observable.Count - 1;

                Observable.RemoveAt(0);
                Observable.Add(ProductDetails.BuildOne(++_maxId));

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