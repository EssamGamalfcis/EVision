using Domain;
using Repository;
using Service.Helper;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Service.Controllers
{
    [ApiController]
    [Route("api/ProductCatalog")]
    public class ProductCatalogController : ControllerBase , IProductCatalog
    {
        IGenericRepository<Product> productRepository;
        public static IWebHostEnvironment _environment;
        public ProductCatalogController(IGenericRepository<Product> _productRepository, IWebHostEnvironment env)
        {
            productRepository = _productRepository;
            _environment = env;
        }
        [Route("~/api/Create")]
        [HttpPost]
        public async Task<StandardResponse> Create([FromForm]Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.IsDeleted = false;
                    product.LastUpdated = DateTime.Now;
                    if (product.PhotoFile != null)
                    {
                        GlobalMethods globalMethods = new GlobalMethods(_environment);
                        GlobalMethods.FIleUploadAPI fileToUpload = new GlobalMethods.FIleUploadAPI() { files= product.PhotoFile};
                        product.PhotoName = await globalMethods.UploadPhoto(fileToUpload);
                    }
                    productRepository.Add(product);
                    return new StandardResponse { Message = "Saved Success", Success = true };
                }
                else
                { 
                return new StandardResponse { Message = "Process failed (Model not match)", Success = false };
                }
            }
            catch (Exception e)
            {
                return new StandardResponse { Message = "Process failed", Success = false };
            }
        }
        [Route("~/api/Delete")]
        [HttpDelete]
        public async Task<StandardResponse> Delete([FromHeader]long id)
        {
            try
            {
                Product productToDelete = GetProductById(id).Product;
                productToDelete.IsDeleted = true;
                await Edit(productToDelete);
                return new StandardResponse { Message = "Deleted Successfully", Success = true };
            }
            catch (Exception e)
            {
                return new StandardResponse { Message = "Process failed", Success = false };
            }
        }
        [Route("~/api/Edit")]
        [HttpPut]
        public async Task<StandardResponse> Edit([FromForm]Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.LastUpdated = DateTime.Now;
                    if (product.PhotoFile != null)
                    {
                        GlobalMethods globalMethods = new GlobalMethods(_environment);
                        GlobalMethods.FIleUploadAPI fileToUpload = new GlobalMethods.FIleUploadAPI() { files = product.PhotoFile };
                        product.PhotoName = await globalMethods.UploadPhoto(fileToUpload);
                    }
                    productRepository.Update(product);
                    return new StandardResponse { Message = "Updated Successfully", Success = true };
                }
                else
                {
                    return new StandardResponse { Message = "Process failed (Model not match)", Success = false };
                }

            }
            catch (Exception e)
            { 
                return new StandardResponse { Message = "Process failed", Success = false };
            }
        }
        [Route("~/api/GetAllProducts")]
        [HttpGet]
        public ProductsReturn GetAllProducts([FromQuery]PagingParam param)
        {
            try
            {
                IQueryable<Product> products = productRepository.GetAllQ();
                return new ProductsReturn
                {
                    Products = products.Where(x=>x.IsDeleted != true).
                    Skip((param.PageCount)*(--param.PageNumber)).Take(param.PageCount).ToList(),
                    TotalCount = products.Count(x => x.IsDeleted != true),
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
        [Route("~/api/GetProductById")]
        [HttpGet]
        public ProductReturn GetProductById([FromQuery]long id)
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
        [Route("~/api/Search")]
        [HttpGet]
        public ProductsReturn Search([FromQuery]SearchingWithPagingParam param)
        {
            try
            {
                IQueryable<Product> products = productRepository.GetAllQ();

                return new ProductsReturn
                {
                    Products = products.Where(x=>x.Name.Contains(param.productName) && x.IsDeleted != true).
                                Skip((param.PageCount) * (--param.PageNumber)).Take(param.PageCount).ToList(),
                    TotalCount = products.Count(x =>x.Name.Contains(param.productName) &&  x.IsDeleted != true),
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

        [Route("~/api/ExportToExcell")]
        [HttpPost]
        public IActionResult ExportToExcel([FromBody]ExportProductsParam products)
        {
            try
            {
                GlobalMethods globalMethods = new GlobalMethods(_environment);
                PagingParam pagingParam = new PagingParam() { PageCount = products.PageCount, PageNumber = products.PageNumber };
                products.products = GetAllProducts(pagingParam).Products;
                return globalMethods.ExportToExcel(this, products.products);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
