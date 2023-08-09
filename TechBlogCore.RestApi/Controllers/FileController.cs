using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechBlogCore.RestApi.Dtos;
using TechBlogCore.RestApi.Helpers;

namespace TechBlogCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IConfiguration config;

        public FileController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetFile(string name)
        {
            var path = config["UploadFilePath"];
            var location = Path.Combine(path, name);
            if (!System.IO.File.Exists(location)) return NotFound();
            var ext = name.Substring(name.LastIndexOf('.')).ToLower();
            if (ext != ".jpg" && ext != ".png") return NotFound();
            return File(await System.IO.File.ReadAllBytesAsync(location), ext.Replace(".", "image/"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            if (Request.Form.Files.Count == 0)
                throw new MessageException("无有效文件!");

            var files = Request.Form.Files;

            foreach (var file in files)
            {
                var ext = file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower();
                if ((ext != ".jpg" && ext != ".png") || !file.ContentType.Contains("image"))
                {
                    throw new MessageException("图片格式不正确");
                }
            }

            var path = config["UploadFilePath"];
            var results = new List<string>(files.Count);
            foreach (var file in files)
            {
                var filename = $"{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}{file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower()}";
                var location = Path.Combine(path, filename);
                using (var stream = new FileStream(location, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    stream.Position = 0;
                    await file.OpenReadStream().CopyToAsync(stream);
                }
                results.Add(filename);
            }
            return Ok(string.Join(",", results));
        }
    }
}
