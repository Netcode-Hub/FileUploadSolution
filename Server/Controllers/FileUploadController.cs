using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController(IWebHostEnvironment webHostEnvironment) : ControllerBase
    {
        [HttpPost("uploads")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "Images");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            var imageUrl = $"{Request.Scheme}://{Request.Host}/Images/{fileName}";
            return Ok(imageUrl);
        }
    }
}
