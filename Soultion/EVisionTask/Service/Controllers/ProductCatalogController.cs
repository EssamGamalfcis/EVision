using Domain;
using Repository;
using Service.Helper;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
namespace Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductCatalogController : IProductCatalog
    {
        IGenericRepository<Product> productRepository;
        public ProductCatalogController(IGenericRepository<Product> _productRepository)
        {
            productRepository = _productRepository;
        }
        [HttpPost]
        public StandardResponse Create(Product product)
        {
            try
            {
                productRepository.Add(product);
                return new StandardResponse { Message = "Saved Success" , Success =true };
            }
            catch (Exception e)
            {
                return new StandardResponse { Message = "Process failed", Success = false };
            }
        }
        [HttpDelete]
        public StandardResponse Delete(long id)
        {
            try
            {
                Product productToDelete = GetProductById(id).Product;
                productToDelete.IsDeleted = true;
                productToDelete.LastUpdated = DateTime.Now;
                Edit(productToDelete);
                return new StandardResponse { Message = "Deleted Successfully", Success = true };
            }
            catch (Exception e)
            {
                return new StandardResponse { Message = "Process failed", Success = false };
            }
        }
        [HttpPut]
        public StandardResponse Edit(Product product)
        {
            try
            {
                productRepository.Update(product);
                return new StandardResponse { Message = "Updated Successfully", Success = true };
            }
            catch (Exception e)
            { 
                return new StandardResponse { Message = "Process failed", Success = false };
            }
        }
        [HttpGet]
        public ProductsReturn GetAllProducts(PagingParam param)
        {
            try
            {
                IQueryable<Product> products = productRepository.GetAllQ();
                return new ProductsReturn
                {
                    Products = products.Where(x=>x.IsDeleted != true).
                    Skip((param.PageCount--)*(param.PageCount)).Take(param.PageCount).ToList(),
                    Message = "Process success",
                    Success = true
                };
            }
            catch
            {
                return new ProductsReturn
                {
                    Products = null,
                    Message = "Process failed",
                    Success = false
                };

            }
        }
        [HttpGet]
        public ProductReturn GetProductById(long id)
        {
            try
            {
                return new ProductReturn
                {
                    Product = productRepository.FindBy(x => x.Id == id && x.IsDeleted != true).FirstOrDefault(),
                    Message = "Process success",
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ProductReturn
                {
                    Product = null,
                    Message = "Process failed",
                    Success = false
                };
            }
        }
        [HttpGet]
        public ProductsReturn Search(SearchingWithPagingParam param)
        {
            try
            {
                IQueryable<Product> products = productRepository.GetAllQ();

                return new ProductsReturn
                {
                    Products = products.Where(x=>x.Name == param.productName && x.IsDeleted != true).
                                Skip((param.PageCount--) * (param.PageCount)).Take(param.PageCount).ToList(),
                    Message = "Process success",
                    Success = true
                };
            }
            catch
            {
                return new ProductsReturn
                {
                    Products = null,
                    Message = "Process failed",
                    Success = false
                };

            }
        }
    }
}
