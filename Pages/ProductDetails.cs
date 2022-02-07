using System;
using System.ComponentModel;

namespace MyBlazorServerApp.Pages
{
    public class ProductDetails : INotifyPropertyChanged
    {
        private int _sold;
        private double _amount;
        private string _year;
        private string _country;
        private string _product;
        private string _quarter;

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
        
        public string Country
        {
            get => _country; 
            set
            {
                _country = value;
                NotifyPropertyChanged(nameof(Country));
            }
        }

        public string Product
        {
            get => _product;
            set
            {
                _product = value;
                NotifyPropertyChanged(nameof(Product));
            }
        }

        public string Quarter
        {
            get => _quarter;
            set
            {
                _quarter = value;
                NotifyPropertyChanged(nameof(Quarter));
            }
        }

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

        public ProductDetails WithValue(ProductDetails pd)
        {
            this.Sold = pd.Sold;
            this.Amount = pd.Amount;
            this.Country = pd.Country;
            this.Product = pd.Product;
            this.Year = pd.Year;
            this.Quarter = pd.Quarter;
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