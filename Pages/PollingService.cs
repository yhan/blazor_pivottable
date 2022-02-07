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
        private const int Size = 2000;
        private const int TimerIntervalSecond = 2;

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
            Observable = new ObservableCollection<ProductDetails>(DataLayer.FetchNew(Size));
            _timer = new Timer(Refresh, null, TimeSpan.FromSeconds(TimerIntervalSecond), TimeSpan.FromSeconds(TimerIntervalSecond));
            //Refresh(null);
        }

        private void Refresh(object? state)
        {
            if(_paused)
                return;

            var nd = DataLayer.FetchNew(Size);
            for (var i = 0; i < Observable.Count; i++)
            {
                Observable[i].WithValue(nd[i]);
            }

            Observable.RemoveAt(0);
            Observable.Add(ProductDetails.BuildOne(42));

            foreach (var handler in _handlers.Values)
            {
                handler();
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

        public ObservableCollection<ProductDetails> Debug(int count)
        {
            var col = new ObservableCollection<ProductDetails>();
            for (var index = 0; index < count; index++)
            {
                
                col.Add(ProductDetails.BuildOne(index));
            }

            return col;
        }
    }
}