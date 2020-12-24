using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Helper;

namespace EVisionTask.Controllers
{
    public class ProductController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        //Hosted web API REST Service base url  
        string Baseurl = "http://localhost:55468/";
        public async Task<ActionResult> Index(SearchingWithPagingParam obj)
        {
            ProductsReturn retObj = new ProductsReturn();
            if (obj.PageCount == 0 && obj.PageNumber == 0)
            {
                obj.PageCount = 5;
                obj.PageNumber = 1;
            }

            ViewBag.PageNumber = obj.PageNumber;
            ViewBag.PageSize = obj.PageCount;
            ViewBag.SearchKey = null;
            /*check whether search or get all*/
            string apiURL = "";
            if (obj.ProductName == null || obj.ProductName == "" || obj.ProductName == "undefined")
            {
                apiURL = "api/GetAllProducts?PageNumber=" + obj.PageNumber + "&PageCount=" + obj.PageCount;
            }
            else
            {
                apiURL = "api/Search?PageNumber=" + obj.PageNumber + "&PageCount=" + obj.PageCount + "&ProductName=" + obj.ProductName;
                ViewBag.SearchKey = obj.ProductName;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(apiURL);
                if (Res.IsSuccessStatusCode)
                {
                    var productsResponse = Res.Content.ReadAsStringAsync().Result;
                    retObj = JsonConvert.DeserializeObject<ProductsReturn>(productsResponse);

                }
                return View(retObj);
            }

        }
        public async Task<ActionResult> Create(Product product)
        {
            StandardResponse retObj = new StandardResponse();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                var Res = await client.PostAsJsonAsync<Product>("product", product);
                if (Res.IsSuccessStatusCode)
                {
                    var createProductResponse = Res.Content.ReadAsStringAsync().Result;
                    retObj = JsonConvert.DeserializeObject<StandardResponse>(createProductResponse);
                }
            }

            return View(product);
        }
        public async Task<ActionResult> Edit(Product product)
        {
            StandardResponse retObj = new StandardResponse();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                var Res = await client.PutAsJsonAsync<Product>("product", product);
                if (Res.IsSuccessStatusCode)
                {
                    var createProductResponse = Res.Content.ReadAsStringAsync().Result;
                    retObj = JsonConvert.DeserializeObject<StandardResponse>(createProductResponse);
                }
            }

            return View(product);
        }
        [HttpDelete]
        public async Task<bool> Delete(long id)
        {
            StandardResponse retObj = new StandardResponse();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Add("id", id.ToString());
                var Res = await client.DeleteAsync("api/Delete");
                if (Res.IsSuccessStatusCode)
                {
                    var createProductResponse = Res.Content.ReadAsStringAsync().Result;
                    retObj = JsonConvert.DeserializeObject<StandardResponse>(createProductResponse);
                    if (retObj.Success == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
