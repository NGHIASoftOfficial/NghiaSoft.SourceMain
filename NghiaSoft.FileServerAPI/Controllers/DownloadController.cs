using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NghiaSoft.FileServerAPI.Data;
using NghiaSoft.FileServerAPI.Data.Entities;

namespace NghiaSoft.FileServerAPI.Controllers;

[ApiController]
[Route("/download")]
public class DownloadController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public DownloadController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        // Lấy thư mục gốc từ appsettings.json
        var rootFolder = _configuration["RootFileFolder"];
        if (string.IsNullOrWhiteSpace(rootFolder))
        {
            return BadRequest("RootFileFolder configuration is missing.");
        }

        var fi = await _context.AppFileInfos.FirstOrDefaultAsync(x => x.FileName == fileName);
        if (fi == null)
        {
            return NotFound("File not found.");
        }

        if (fi.IsAuthenticateRequired && User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized("Please verify your identity.");
        }

        var crrUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (fi.IsOwnedVisibleOnly && crrUserId != fi.UploaderId)
        {
            return Forbid("You do not permitted.");
        }

        // Đường dẫn tới tệp cần tải về
        var filePath = Path.Combine(rootFolder, fi.Id.ToString("N"));

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found.");
        }

        try
        {
            var fiad = new AppFileActionDetail
            {
                Id = Guid.NewGuid(),
                FileId = fi.Id,
                ActionType = "D",
                CreationTime = DateTime.UtcNow,
                ActorId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                ActorUserName = User.Identity?.Name,
            };
            await _context.AppFileActionDetails.AddAsync(fiad);
            await _context.SaveChangesAsync();

            // Tạo PhysicalFileResult để trả về tệp
            return PhysicalFile(filePath, "application/octet-stream", enableRangeProcessing: true,
                fileDownloadName: fi.FileName);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error downloading file: {ex.Message}");
        }
    }
}