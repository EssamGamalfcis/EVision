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
            ViewBag.SavedUpdated = "Not Set";
            if (TempData["Saved"] != null)
            {
                if (TempData["Saved"].ToString() == "Saved Successfully")
                {
                    ViewBag.SavedUpdated = "Saved Successfully";
                }
                else if (TempData["Saved"].ToString() == "Updated Successfully")
                {
                    ViewBag.SavedUpdated = "Updated Successfully";
                }
                else
                {
                    ViewBag.SavedUpdated = "Process failed";
                }
            }
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
            using (var content = new MultipartFormDataContent())
            {
                var values = new[]
                {
                    new KeyValuePair<string, string>("Name", product.Name),
                    new KeyValuePair<string, string>("Price", product.Price.ToString())
                };

                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                }
                if (product.PhotoFile != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(product.PhotoFile.ContentDisposition).FileName.Trim('"');
                    content.Add(new StreamContent(product.PhotoFile.OpenReadStream())
                    {
                        Headers =
                    {
                        ContentLength = product.PhotoFile.Length,
                        ContentType = new MediaTypeHeaderValue(product.PhotoFile.ContentType)
                    }
                    }, "PhotoFile", fileName);
                }
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    var Res = await client.PostAsync("api/Create", content);
                    if (Res.IsSuccessStatusCode)
                    {
                        var createProductResponse = Res.Content.ReadAsStringAsync().Result;
                        retObj = JsonConvert.DeserializeObject<StandardResponse>(createProductResponse);
                        if (retObj.Message != "Process failed")
                        {
                            TempData["Saved"] = "Saved Successfully";
                        }
                        else
                        {
                            TempData["Saved"] = "Process failed";
                        }
                    }
                }
            }

            return RedirectToAction("Index");

        }
        public async Task<ActionResult> Edit(Product product, [FromForm] SearchingWithPagingParam obj)
        {
            StandardResponse retObj = new StandardResponse();
            using (var content = new MultipartFormDataContent())
            {
                var values = new[]
                {
                    new KeyValuePair<string, string>("Id", product.Id.ToString()),
                    new KeyValuePair<string, string>("Name", product.Name),
                    new KeyValuePair<string, string>("Price", product.Price.ToString())
                };

                foreach (var keyValuePair in values)
                {
                    content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                }
                if (product.PhotoFile != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(product.PhotoFile.ContentDisposition).FileName.Trim('"');
                    content.Add(new StreamContent(product.PhotoFile.OpenReadStream())
                    {
                        Headers =
                    {
                        ContentLength = product.PhotoFile.Length,
                        ContentType = new MediaTypeHeaderValue(product.PhotoFile.ContentType)
                    }
                    }, "PhotoFile", fileName);
                }
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    var Res = await client.PutAsync("api/Edit", content);
                    if (Res.IsSuccessStatusCode)
                    {
                        var createProductResponse = Res.Content.ReadAsStringAsync().Result;
                        retObj = JsonConvert.DeserializeObject<StandardResponse>(createProductResponse);
                        if (retObj.Message != "Process failed")
                        {
                            TempData["Saved"] = "Updated Successfully";
                        }
                        else
                        {
                            TempData["Saved"] = "Process failed";
                        }
                    }
                }
            }
            return RedirectToAction("Index", obj);
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
