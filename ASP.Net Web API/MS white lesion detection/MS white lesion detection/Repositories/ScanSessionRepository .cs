using MS_white_lesion_detection.Models;
using MS_white_lesion_detection.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MS_white_lesion_detection.Repositories
{
    public class ScanSessionRepository:GenericRepository<ScanSession>, IScanSessionRepository
    {
        private readonly AppDbContext _context;

        public ScanSessionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ScanSession?> GetByUserIdAsync(int userId)
        {
            return await _context.ScanSessions
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

    }
}
