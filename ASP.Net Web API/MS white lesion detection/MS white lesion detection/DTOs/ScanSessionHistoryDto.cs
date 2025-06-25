namespace MS_white_lesion_detection.DTOs
{
    public class ScanSessionHistoryDto
    {
        public int SessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ReportText { get; set; }
        public List<string> MaskPaths { get; set; }
        public List<string> ScanFilePaths { get; set; }
    }
}
