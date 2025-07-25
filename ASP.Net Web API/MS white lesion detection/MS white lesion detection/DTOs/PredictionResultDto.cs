using MS_white_lesion_detection.Models;

namespace MS_white_lesion_detection.DTOs
{
    public class PredictionResultDto
    {
        public string ReportText { get; set; }
        public List<PredictionMaskDto> PredictionMasks { get; set; }
    }
}
