using Microsoft.AspNetCore.Mvc;

namespace SpaceMarineAPI.Controllers
{
    
    [ApiController]
        [Route("api/[controller]")]
        public class UploadController : ControllerBase
        {
            private readonly IWebHostEnvironment _env;

            public UploadController(IWebHostEnvironment env)
            {
                _env = env;
            }

            [HttpPost]
            public async Task<IActionResult> Upload(IFormFile file)
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { filename = uniqueFileName });
            }
        }
}
