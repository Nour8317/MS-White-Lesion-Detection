using MS_white_lesion_detection.Data;
using MS_white_lesion_detection.Interfaces;
using MS_white_lesion_detection.Models;

namespace MS_white_lesion_detection.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IGenericRepository<Scan> Scans { get; }
        public IScanSessionRepository ScanSessions { get; }
        public IGenericRepository<Prediction> Predictions { get; }
        public IGenericRepository<PredictionMask> PredictionMasks { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Scans = new GenericRepository<Scan>(_context);
            ScanSessions = new ScanSessionRepository(_context);
            Predictions = new GenericRepository<Prediction>(_context);
            PredictionMasks = new GenericRepository<PredictionMask>(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    }
}
