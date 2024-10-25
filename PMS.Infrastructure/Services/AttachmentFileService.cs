using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PMS.Application.Interfaces;

namespace PMS.Infrastructure.Services
{
    public class AttachmentFileService : IAttachmentFileService
    {
        private readonly string _uploadPath;

        public AttachmentFileService(IConfiguration configuration)
        {
            // Set upload directory to wwwroot/upload, ensuring directory is created if not exists
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
            Directory.CreateDirectory(_uploadPath); // Ensures upload folder exists
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            // Generate unique filename (you can customize this if needed)
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Return the file path for reference in domain logic
        }
    }
}