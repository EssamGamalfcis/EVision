using ClosedXML.Excel;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace Service.Helper
{
    public class GlobalMethods
    {
        public static IWebHostEnvironment _environment;
        public GlobalMethods(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public class FIleUploadAPI
        {
            public IFormFile files { get; set; }
        }
        public async Task<string> UploadPhoto(FIleUploadAPI files)
        {
            if (files.files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\inetpub\\wwwroot\\uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\inetpub\\wwwroot\\uploads\\");
                    }
                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\inetpub\\wwwroot\\uploads\\" + files.files.FileName))
                    {
                        files.files.CopyTo(filestream);
                        filestream.Flush();
                        return "\\uploads\\" + files.files.FileName;
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "Unsuccessful";
            }

        }

        public IActionResult ExportToExcel(ControllerBase controller, List<Product> dataToExport)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "#";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Price";
                worksheet.Cell(currentRow, 4).Value = "LastUpdated";
                worksheet.Cell(currentRow, 5).Value = "PhotoName";
                foreach (var singleRow in dataToExport)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = currentRow-1;
                    worksheet.Cell(currentRow, 2).Value = singleRow.Name;
                    worksheet.Cell(currentRow, 3).Value = singleRow.Price;
                    worksheet.Cell(currentRow, 4).Value = singleRow.LastUpdated;
                    worksheet.Cell(currentRow, 5).Value = singleRow.PhotoName;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return controller.File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Products.xlsx");
                }
            }
        }

    }
}
