using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace MyBlazorServerApp.Pages
{
    public class ProductDetails : INotifyPropertyChanged
    {
        private int _sold;
        private double _amount;
        private string _year;

        public int Sold
        {
            get => _sold;
            set
            {
                _sold = value;
                NotifyPropertyChanged(nameof(Sold));
            }
        }

        public double Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                NotifyPropertyChanged(nameof(Amount));
            }
        }

        public string Year
        {
            get => _year;
            set
            {
                _year = value;
                NotifyPropertyChanged(nameof(Year));
            }
        }

        public string Country { get; set; }
        public string Product { get; set; }
        public string Quarter { get; set; }

        public static string[] Countries = new string[] { "France", "Germany", "United States" };
        public static string[] Prds = new string[] { "Mountain Bikes", "Road Bikes" };
        public static string[] Yrs = new string[] { "FY 2015", "FY 2016", "FY 2017" };
        public static string[] Qters = new string[] { "Q1", "Q2", "Q3", "Q4" };

        private static Random Rand = new Random(42);


        public static List<ProductDetails> GetProductData(int nbProducts)
        {
            var productData = new List<ProductDetails>();

            for (int i = 0; i < nbProducts; i++)
            {
                productData.Add(BuildOne(i));
            }
            return productData;
        }

        public static ProductDetails BuildOne(int id)
        {
            return new ProductDetails
            {
                Id = id,
                Sold = Rand.Next(100),
                Amount = Rand.Next(100),
                Country = Countries[Rand.Next(0, Countries.Length)],
                Product = Prds[Rand.Next(0, Prds.Length)],
                Year = Yrs[Rand.Next(0, Yrs.Length)],
                Quarter = Qters[Rand.Next(0, Qters.Length)],
            };
        }

        public int Id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class PollingService
    {
        private Timer timer;
        public ProductDetails[] Cache;

        public static PollingService Instance = new PollingService();

        public PollingService()
        {
            timer = new Timer(Refresh, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
            Cache = new ProductDetails[200];
        }

        private void Refresh(object? state)
        {
            if(_paused)
                return;

            for (var i = 0; i < Cache.Length; i++)
            {
                Cache[i] = ProductDetails.BuildOne(i);
            }
            Debug.WriteLine($"handlers count = {_handlers.Count}");
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