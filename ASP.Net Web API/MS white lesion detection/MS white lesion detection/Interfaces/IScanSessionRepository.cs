using MS_white_lesion_detection.Interfaces;
using MS_white_lesion_detection.Models;

namespace MS_white_lesion_detection.Repositories
{
    public interface IScanSessionRepository : IGenericRepository<ScanSession>
    {
        public Task<ScanSession?> GetByUserIdAsync(int userId);
    }
}