using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS_white_lesion_detection.Models;
using MS_white_lesion_detection.DTOs;
using MS_white_lesion_detection.Interfaces;

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
    public async Task<IActionResult> GetPredictionBySession(int id)
    {
        var prediction = (await _unitOfWork.Predictions.GetAllAsync()).FirstOrDefault(p => p.SessionId == id);
        if (prediction == null) return NotFound();

        var masks = (await _unitOfWork.PredictionMasks.GetAllAsync()).Where(m => m.PredictionId == prediction.Id).ToList();
        return Ok(new { prediction, masks });
    }
}
