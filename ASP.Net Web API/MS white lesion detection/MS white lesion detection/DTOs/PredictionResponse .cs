using System.Collections.Generic;

namespace MS_white_lesion_detection.DTOs
{
    public class PredictionResponse
    {
        public string Report { get; set; }
        public Dictionary<string, string> Overlays { get; set; }
    }

}
