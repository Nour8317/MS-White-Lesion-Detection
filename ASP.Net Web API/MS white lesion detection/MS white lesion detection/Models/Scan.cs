using System.ComponentModel.DataAnnotations.Schema;

namespace MS_white_lesion_detection.Models
{
    public class Scan
    {
        public int Id { get; set; }
        public int ScanSessionId { get; set; }
        public string FilePath { get; set; }
        [NotMapped]
        public IFormFile Scanimg { get; set; }
        [ForeignKey("ScanSessionId")]
        public ScanSession ScanSession { get; set; }
    }
}