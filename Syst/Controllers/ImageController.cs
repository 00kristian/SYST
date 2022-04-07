using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private static string DESTINATION = "Images/";

        public static Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public ImageController(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _environment = environment;
        }
    
        [HttpPost]
        public string Post(IFormFile file)
        {
            var name = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

            if (file.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.ContentRootPath + DESTINATION))
                     {
                        Directory.CreateDirectory(_environment.ContentRootPath + DESTINATION);
                    }
                    using (FileStream filestream = System.IO.File.Create(_environment.ContentRootPath + DESTINATION + name))
                    {
                        file.CopyTo(filestream);
                        filestream.Flush();
                        return name;
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
        [HttpGet("{filename}")]
        public IActionResult Get(string filename)
        {
            System.Console.WriteLine(_environment.ContentRootPath + "Images/" + filename);
            var image = System.IO.File.OpenRead(_environment.ContentRootPath + "Images/" + filename);
            return File(image, "image/" + System.IO.Path.GetExtension(filename).Remove(0, 1));
        }   
    }
}