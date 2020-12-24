using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Helper
{
    public class ViewModels
    {
    }

    public class PagingParam
    {
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
    }
    public class SearchingWithPagingParam : PagingParam
    {
        public string ProductName { get; set; }
    }
    public class ExportProductsParam : PagingParam
    {
       public List<Product> Products { get; set; }
    }
}
