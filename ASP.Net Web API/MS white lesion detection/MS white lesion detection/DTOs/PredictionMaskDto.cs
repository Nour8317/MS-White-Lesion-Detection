using MS_white_lesion_detection.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS_white_lesion_detection.DTOs
{
    public class PredictionMaskDto
    {
        public int Id { get; set; }
        public int PredictionId { get; set; }
        public string MaskPath { get; set; }
    }
}
