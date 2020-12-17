using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Helper
{
    public class StandardResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class ProductReturn : StandardResponse
    { 
        public Product Product { get; set; }
    }
    public class ProductsReturn : StandardResponse
    {
        public List<Product> Products { get; set; }
    }
}
