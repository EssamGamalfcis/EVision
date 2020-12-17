using Domain;
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
        StandardResponse Edit(Product product);
        StandardResponse Delete(long id);
        StandardResponse Create(Product product);
    }
}
