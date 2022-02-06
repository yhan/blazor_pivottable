//using System;
//using System.Collections.ObjectModel;
//using System.ComponentModel;

//namespace MyBlazorServerApp.Pages
//{
//    public class ProductDetails2 : INotifyPropertyChanged
//    {
//        private int _sold;
//        private double _amount;
//        private string _year;

//        public int Sold
//        {
//            get => _sold;
//            set
//            {
//                _sold = value;
//                NotifyPropertyChanged(nameof(Sold));
//            }
//        }

//        public double Amount
//        {
//            get => _amount;
//            set
//            {
//                _amount = value;
//                NotifyPropertyChanged(nameof(Amount));
//            }
//        }

//        public string Year
//        {
//            get => _year;
//            set
//            {
//                _year = value;
//                NotifyPropertyChanged(nameof(Year));
//            }
//        }

//        public string Country { get; set; }

//        public string Product { get; set; }

//        public string Quarter { get; set; }

//        public static string[] Countries = new string[] { "France", "Germany", "United States" };
//        public static string[] Prds = new string[] { "Mountain Bikes", "Road Bikes" };
//        public static string[] Yrs = new string[] { "FY 2015", "FY 2016", "FY 2017" };
//        public static string[] Qters = new string[] { "Q1", "Q2", "Q3", "Q4" };

//        private static Random Rand = new Random(42);


//        public static ObservableCollection<ProductDetails2> GetProductData(int nbProducts)
//        {
//            var productData = new ObservableCollection<ProductDetails2>();

//            for (int i = 0; i < nbProducts; i++)
//            {
//                productData.Add(BuildOne(i));
//            }
//            return productData;
//        }

//        public static ProductDetails2 BuildOne(int id)
//        {
//            return new ProductDetails2
//            {
//                Id = id,
//                Sold = Rand.Next(100),
//                Amount = Rand.Next(1000),
//                Country = Countries[Rand.Next(0, Countries.Length)],
//                Product = Prds[Rand.Next(0, Prds.Length)],
//                Year = Yrs[Rand.Next(0, Yrs.Length)],
//                Quarter = Qters[Rand.Next(0, Qters.Length)],
//            };
//        }

//        public int Id { get; set; }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private void NotifyPropertyChanged(string propertyName)
//        {
//            var handler = PropertyChanged;
//            if (handler != null)
//            {
//                handler(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//    }
//}