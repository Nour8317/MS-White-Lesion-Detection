using System.ComponentModel.DataAnnotations.Schema;

namespace MS_white_lesion_detection.Models
{
    public class PredictionMask
    {
        public int Id { get; set; }
        public int PredictionId { get; set; }
        public string MaskPath { get; set; }
        [NotMapped]
        public IFormFile Mask {  get; set; }
        public Prediction Prediction { get; set; }
    }
}