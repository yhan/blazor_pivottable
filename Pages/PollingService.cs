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
        private Timer timer;
        public ObservableCollection<ProductDetails> Observable { get; set; }

        private static PollingService _instance;
        public static PollingService Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new PollingService(new ProductDetails[200_000]);
                }

                return _instance;
            }
        } 

        private readonly Dictionary<int, Action> _handlers = new Dictionary<int, Action>();
        private bool _paused;


        public PollingService(ProductDetails[] arr)
        {
            Observable = new ObservableCollection<ProductDetails>(arr);
            timer = new Timer(Refresh, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            //Refresh(null);
        }

        private void Refresh(object? state)
        {
            if(_paused)
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

            //Debug.WriteLine($"handlers count = {_handlers.Count}");
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
    }
}