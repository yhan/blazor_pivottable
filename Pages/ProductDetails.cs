using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        public ProductDetails Random()
        {
            this.Sold = Rand.Next(100);
            this.Amount = Rand.Next(100);
            this.Country = Countries[Rand.Next(0, Countries.Length)];
            this.Product = Prds[Rand.Next(0, Prds.Length)];
            this.Year = Yrs[Rand.Next(0, Yrs.Length)];
            this.Quarter = Qters[Rand.Next(0, Qters.Length)];
            return this;
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
}