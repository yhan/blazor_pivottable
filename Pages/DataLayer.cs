using System;

namespace MyBlazorServerApp.Pages
{
    internal class DataLayer
    {
        private static Random _rand = new Random();
        public static ProductDetails[] FetchNew(int size)
        {
            //debug
            if (size == 1)
                return new[]
                {
                    ProductDetails.BuildOne(1),
                };

            var ns = _rand.Next((int)(size * 0.8), (int)(size * 1.2));
            var arr = new ProductDetails[ns];
            for (int i = 0; i<arr.Length; i++)
            {
                arr[i] = ProductDetails.BuildOne(i);
            }

            return arr;
        }
    }
}