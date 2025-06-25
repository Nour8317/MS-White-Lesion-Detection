namespace MS_white_lesion_detection.Models
{
    public class Prediction
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string ReportText { get; set; }

        public ScanSession ScanSession { get; set; }
        public ICollection<PredictionMask> PredictionMasks { get; set; }
    }
}