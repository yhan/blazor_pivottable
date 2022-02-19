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
        
        private readonly object _syncRoot = new object();
        private const int Size = 200_000;
        private const int TimerIntervalSecond = 10;

        private static PollingService _instance;

        public static PollingService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PollingService();
                }

                return _instance;
            }
        }

        private readonly Dictionary<int, Action> _handlers = new Dictionary<int, Action>();
        private bool _paused;


        public PollingService()
        {
            Observable = new ObservableCollection<ProductDetails>(DataLayer.FetchNew(Size));
            _timer = new Timer(Refresh, null, TimeSpan.FromSeconds(TimerIntervalSecond), TimeSpan.FromSeconds(TimerIntervalSecond));
        }

        private void Refresh(object? state)
        {
            if (_paused)
                return;


            var nd = DataLayer.FetchNew(Size);
            var newSize = nd.Length;
            var prevSize = Observable.Count;

            var replaceSize = Math.Min(newSize, prevSize);

            for (var i = 0; i < replaceSize; i++)
            {
                Observable[i].WithValue(nd[i]);
            }

            if (newSize < prevSize)
            {
                var i = prevSize;
                for (i = prevSize - 1; i >= replaceSize; i--)
                {
                    Observable.RemoveAt(i);
                }

                Debug.WriteLine($"removed {prevSize - newSize}");
            }
            else if (newSize > prevSize)
            {
                for (int i = prevSize; i < replaceSize; i++)
                {
                    Observable.Add(nd[i]);
                }

                Debug.WriteLine($"added {newSize - prevSize}");
            }

            //Observable.Add(ProductDetails.BuildOne(42));

            lock (_syncRoot)
            {
                Debug.WriteLine($"handlers count = {_handlers.Count}");
                foreach (var handler in _handlers.Values)
                {
                    handler();
                }
            }

        }

        public void Register(int subscriber, Action refreshData)
        {
            lock (_syncRoot)
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
        
        public void UnRegister(int subscriber)
        {
            lock (_syncRoot)
                _handlers.Remove(subscriber);
        }
    }
}