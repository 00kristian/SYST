using System;
using System.IO;
using System.Threading.Tasks;
using Core;
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

        public IQuestionRepository _repo;

        public ImageController(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, IQuestionRepository repo)
        {
            _environment = environment;

            _repo = repo;
        }
    
        [HttpPost("{questionid}")]
        public async Task<IActionResult> Post(int questionid, IFormFile file)
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
                        var res = await _repo.UpdateImage(questionid, name);
                        if (res == Status.NotFound) return res.ToActionResult();
        
                        return CreatedAtAction(nameof(Get), new { name }, name);
                    }
                }
                catch (Exception)
                {
                    return Status.BadRequest.ToActionResult();
                }
            }
            else
            {
                return Status.BadRequest.ToActionResult();
            }

        }
        [HttpGet("{filename}")]
        public IActionResult Get(string filename)
        {
            var image = System.IO.File.OpenRead(_environment.ContentRootPath + "Images/" + filename);
            return File(image, "image/" + System.IO.Path.GetExtension(filename).Remove(0, 1));
        }   
    }
}