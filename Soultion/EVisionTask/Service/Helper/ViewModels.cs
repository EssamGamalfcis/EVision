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
        public string productName { get; set; }
    }
}
