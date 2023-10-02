using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private static readonly Random Random = new Random();

        public ImageController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet(Endpoints.ImageController.GetRandomPreview , Name = "GetRandomPreview")]
        public IActionResult GetRandomPreview()
        {
            int random = Random.Next(1, 100);
            string fileName = $"image_{random}.jpg";
            var imagePath = Path.Combine("Resources\\Preview", fileName);
            var imageFileStream = System.IO.File.OpenRead(imagePath);
            return File(imageFileStream, "image/jpeg");
        }

        [HttpGet(Endpoints.ImageController.GetPreviewByNum, Name = "GetPreviewByNum")]
        public IActionResult GetPreviewByNum(int num)
        {
            string fileName = $"image_{num}.jpg";
            var imagePath = Path.Combine("Resources\\Preview", fileName);
            var imageFileStream = System.IO.File.OpenRead(imagePath);
            return File(imageFileStream, "image/jpeg");
        }
    }
}
