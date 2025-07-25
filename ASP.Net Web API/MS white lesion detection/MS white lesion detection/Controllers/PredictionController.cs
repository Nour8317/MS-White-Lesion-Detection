using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS_white_lesion_detection.Models;
using MS_white_lesion_detection.DTOs;
using MS_white_lesion_detection.Interfaces;
using MS_white_lesion_detection.Repositories;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using MS_white_lesion_detection.DTOs;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;


[ApiController]
[Route("api/[controller]")]
public class PredictionController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public PredictionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [Authorize]
    [HttpGet("session/{id}")]
    [ProducesResponseType(typeof(PredictionResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPredictionBySession(int id)
    {
        var prediction = (await _unitOfWork.Predictions.GetAllAsync())
            .FirstOrDefault(p => p.SessionId == id);

        if (prediction == null)
            return NotFound();

        var masks = (await _unitOfWork.PredictionMasks.GetAllAsync())
            .Where(m => m.PredictionId == prediction.Id)
            .ToList();

        var response = new PredictionResultDto
        {
            ReportText = prediction.ReportText,
            PredictionMasks = masks.Select(m => new PredictionMaskDto
            {
                Id = m.Id,
                PredictionId = m.PredictionId,
                MaskPath = m.MaskPath
            }).ToList()
        };

        return Ok(response);
    }




    [Authorize]
    [HttpPost("session/{id}/predict")]
    public async Task<IActionResult> PredictSession(int id)
    {
        var session = await _unitOfWork.ScanSessions.GetByIdAsync(id);
        if (session == null)
            return NotFound("Session not found.");

        var scans = (await _unitOfWork.Scans.GetAllAsync())
            .Where(scan => scan.ScanSessionId == id)
            .ToList();

        if (!scans.Any())
            return BadRequest("No scans found for this session.");

        using var client = new HttpClient(); 
        client.Timeout = TimeSpan.FromMinutes(30);
        using var form = new MultipartFormDataContent();

        int fileIndex = 1;
        foreach (var scan in scans)
        {
            if (!System.IO.File.Exists(scan.FilePath))
                return BadRequest($"Scan file not found: {scan.FilePath}");

            var fileBytes = await System.IO.File.ReadAllBytesAsync(scan.FilePath);
            var content = new ByteArrayContent(fileBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var key = $"file{fileIndex++}";
            form.Add(content, key, Path.GetFileName(scan.FilePath));
        }

        Console.WriteLine("MultipartFormDataContent Details:");
        foreach (var content in form)
        {
            var contentDisposition = content.Headers.ContentDisposition;
            var key = contentDisposition.Name;
            var fileName = contentDisposition.FileName?.Trim('"');
            long? fileSize = content.Headers.ContentLength;

            Console.WriteLine($"Key: {key}, FileName: {fileName}, Size: {fileSize} bytes");
        }

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "3fae583a-878c-4069-b27d-5c636e257ba2");

        var url = "https://8000-dep-01k0wnedrkhj5jnp7fn8fkzjad-d.cloudspaces.litng.ai/predict";
        var response = await client.PostAsync(url, form);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            Console.WriteLine($"Error response: {errorContent}");
            return StatusCode((int)response.StatusCode, errorContent);
        }

        var resultJson = await response.Content.ReadAsStringAsync();

        var predictionResponse = JsonSerializer.Deserialize<PredictionResponse>(resultJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var prediction = new Prediction
        {
            SessionId = id,
            ReportText = string.IsNullOrWhiteSpace(predictionResponse.Report) ? "Report" : predictionResponse.Report,
            ScanSession = session
        };

        await _unitOfWork.Predictions.AddAsync(prediction);
        await _unitOfWork.CompleteAsync();

        var predictionFolder = Path.Combine("wwwroot", "predictions");
        if (!Directory.Exists(predictionFolder))
            Directory.CreateDirectory(predictionFolder);

        var savedMasks = new List<string>();

        foreach (var kv in predictionResponse.Overlays)
        {
            var fileName = $"{kv.Key}_{Guid.NewGuid()}.png";
            var filePath = Path.Combine(predictionFolder, fileName);

            var imageBytes = Convert.FromBase64String(kv.Value);
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            var mask = new PredictionMask
            {
                PredictionId = prediction.Id,
                MaskPath = Path.Combine("predictions", fileName).Replace("\\", "/")
            };

            await _unitOfWork.PredictionMasks.AddAsync(mask);
            savedMasks.Add(mask.MaskPath);
        }

        await _unitOfWork.CompleteAsync();

        return Ok(new
        {
            prediction.ReportText,
            Masks = savedMasks
        });
    }

}
