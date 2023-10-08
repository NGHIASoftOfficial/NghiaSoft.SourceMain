using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NghiaSoft.FileServerAPI.Common;
using NghiaSoft.FileServerAPI.Data;
using NghiaSoft.FileServerAPI.Data.Entities;

namespace NghiaSoft.FileServerAPI.Controllers;

[ApiController]
[Route("api/upload")]
[Authorize]
public class UploadController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public UploadController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("public")]
    public async Task<IActionResult> UploadPublicFile(IFormFile? f, string? description, string? tags)
    {
        return await UploadWithOptions(f, description, tags);
    }

    [HttpPost("protected")]
    public async Task<IActionResult> UploadProtectedFile(IFormFile? f, string? description, string? tags)
    {
        return await UploadWithOptions(f, description, tags, true);
    }

    [HttpPost("owned")]
    public async Task<IActionResult> UploadOwnedFile(IFormFile? f, string? description, string? tags)
    {
        return await UploadWithOptions(f, description, tags, true, true);
    }

    private async Task<IActionResult> UploadWithOptions(IFormFile? f, string? description, string? tags,
        bool isAuthenticateRequired = false, bool isOwnedVisibleOnly = false)
    {
        if (f == null || f.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var rootFolder = _configuration["RootFileFolder"];
        if (string.IsNullOrWhiteSpace(rootFolder))
        {
            return BadRequest("RootFileFolder configuration is missing.");
        }

        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

        var fUuid = Guid.NewGuid();
        // Chuẩn hoá tên tệp
        var normalizedFileName = FileProcessHelper.NormalizeFileName(f.FileName);

        // Đường dẫn tới thư mục lưu trữ
        var storagePath = Path.Combine(rootFolder, fUuid.ToString("N"));

        // Lưu tệp vào thư mục
        await using (var fileStream = new FileStream(storagePath, FileMode.Create))
        {
            await f.CopyToAsync(fileStream);
        }

        var fi = new AppFileInfo
        {
            Id = fUuid,
            FileName = normalizedFileName,
            OriginPath = baseUrl,
            CreationTime = DateTime.UtcNow,
            UploaderId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
            UploaderUserName = User.Identity?.Name,
            Description = description,
            FileTags = tags,
            IsAuthenticateRequired = isOwnedVisibleOnly || isAuthenticateRequired,
            IsOwnedVisibleOnly = isOwnedVisibleOnly
        };

        await _context.AppFileInfos.AddAsync(fi);
        await _context.SaveChangesAsync();

        // Trả về một phản hồi thành công (200 OK) hoặc phản hồi tùy chỉnh khác tùy theo yêu cầu của bạn
        return Ok(fi);
    }
}