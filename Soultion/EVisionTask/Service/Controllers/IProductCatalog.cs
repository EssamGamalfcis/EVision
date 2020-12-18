using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Controllers
{
    interface IProductCatalog
    {
        ProductsReturn GetAllProducts(PagingParam pagingParam);
        ProductReturn GetProductById(long id);
        ProductsReturn Search(SearchingWithPagingParam searchingWithPagingParam);
        Task<StandardResponse> Edit(Product product);
        Task<StandardResponse> Delete(long id);
        Task<StandardResponse> Create(Product product);
        IActionResult ExportToExcel(ExportProductsParam products);
    }
}
