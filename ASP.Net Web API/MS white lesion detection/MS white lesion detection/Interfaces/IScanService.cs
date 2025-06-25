using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MS_white_lesion_detection.Interfaces
{
    public interface IScanService
    {
        Task<string> SaveScanFileAsync(IFormFile file);
    }
}
