namespace MS_white_lesion_detection.DTOs
{
    public class PredictionResultDto
    {
        public string ReportText { get; set; }
        public List<string> MaskPaths { get; set; }
    }
}
