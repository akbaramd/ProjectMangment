using Microsoft.AspNetCore.Http;

namespace PMS.Application.Interfaces
{
    public interface IAttachmentFileService
    {
        /// <summary>
        /// Saves the uploaded file to the specified upload directory and returns the file path.
        /// </summary>
        /// <param name="file">The file to save.</param>
        /// <returns>The path to the saved file.</returns>
        Task<string> SaveFileAsync(IFormFile file);
    }
}