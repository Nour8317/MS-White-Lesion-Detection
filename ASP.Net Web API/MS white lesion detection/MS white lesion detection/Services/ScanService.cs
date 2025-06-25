using MS_white_lesion_detection.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;


namespace MS_white_lesion_detection.Services
{
    public class ScanService : IScanService
    {
        private readonly IWebHostEnvironment _env;

        public ScanService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveScanFileAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads", fileName);
        }
    }
}
