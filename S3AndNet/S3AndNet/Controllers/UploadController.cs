using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using S3AndNet.Interfaces;

namespace S3AndNet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IS3Service _s3Service;
    public UploadController(IS3Service s3Service)
    {
        _s3Service = s3Service;
    }
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            var fileKey = await _s3Service.UploadFileAsync(file);
            return Ok(new { FileKey = fileKey });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
