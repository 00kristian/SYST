using Core;
using Microsoft.AspNetCore.Mvc;

namespace Syst.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private static string DESTINATION = "Images/";

        public static IHostEnvironment _environment = null!;

        public IQuestionRepository _repo;

        public ImageController(IHostEnvironment environment, IQuestionRepository repo)
        {
            _environment = environment;

            _repo = repo;
        }

        //Create a picture
        [HttpPost("{questionid}")]
        public async Task<IActionResult> Post(int questionid, IFormFile file)
        {
            var name = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

            if (file.Length > 0)
            {
                try
                {
                    var imagesPath = _environment.ContentRootPath + DESTINATION;
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }
                    using (FileStream filestream = System.IO.File.Create(imagesPath + name))
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

        //Return a candidate given the picture's file name
        [HttpGet("{filename}")]
        public IActionResult Get(string filename)
        {
            var image = System.IO.File.OpenRead(_environment.ContentRootPath + "Images/" + filename);
            return File(image, "image/" + System.IO.Path.GetExtension(filename).Remove(0, 1));
        }   
    }
}