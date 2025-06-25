using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS_white_lesion_detection.Models
{
    public class ScanSession
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public ICollection<Scan> Scans { get; set; }
        public Prediction Prediction { get; set; }
    }
}