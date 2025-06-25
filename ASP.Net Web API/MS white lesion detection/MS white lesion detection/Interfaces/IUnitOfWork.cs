using MS_white_lesion_detection.Models;
using MS_white_lesion_detection.Repositories;

namespace MS_white_lesion_detection.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Scan> Scans { get; }
        IScanSessionRepository ScanSessions { get; }
        IGenericRepository<Prediction> Predictions { get; }
        IGenericRepository<PredictionMask> PredictionMasks { get; }
        Task<int> CompleteAsync();
    }
}
