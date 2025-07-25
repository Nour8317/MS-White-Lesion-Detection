using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MS_white_lesion_detection.Models;
using MS_white_lesion_detection.DTOs;
using MS_white_lesion_detection.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScanSessionController : ControllerBase
{
    private readonly IScanService _scanService;
    private readonly IUnitOfWork _unitOfWork;

    public ScanSessionController(IScanService scanService, IUnitOfWork unitOfWork)
    {
        _scanService = scanService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadScans([FromForm] ScanUploadDto dto)
    {
        var userIdClaim = User.FindFirst("id")?.Value;
        if (userIdClaim == null){
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
        }

        var userId = int.Parse(userIdClaim);
        Console.WriteLine(userId);
        var session = new ScanSession
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.ScanSessions.AddAsync(session);
        await _unitOfWork.CompleteAsync();

        foreach (var file in dto.ScanFiles)
        {
            var path = await _scanService.SaveScanFileAsync(file);
            path = "wwwroot/" + path.Replace("\\", "/");
            var scan = new Scan
            {
                ScanSessionId = session.Id,
                FilePath = path
            };

            await _unitOfWork.Scans.AddAsync(scan);
        }

        await _unitOfWork.CompleteAsync();

        return Ok(new { session.Id, message = "Scans uploaded successfully." });
    }
}
