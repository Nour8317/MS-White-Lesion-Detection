using Microsoft.AspNetCore.Identity;

namespace MS_white_lesion_detection.Models
{
    public class User : IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; }
        public ICollection<ScanSession> ScanSessions { get; set; }
    }
}
