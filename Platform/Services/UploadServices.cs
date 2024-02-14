using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Platform.Services
{
    public class UploadServices
    {
      

        public static string UploadFile(IFormFile file, string folderPath, IHostingEnvironment hosting)
        {
            try
            {
                // Generate a new GUID for the file
                var addToImage = Guid.NewGuid();

                // Combine the upload folder path with the web root path
                var upload = Path.Combine(hosting.WebRootPath,folderPath);

                // Create the directory if it doesn't exist
                Directory.CreateDirectory(upload);

                // Combine the full path with the generated file name
                var fullPath = Path.Combine(upload, $"{addToImage}_{file.FileName}");

                // Copy the file to the specified location
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Return the generated GUID as a string
                return $"/{folderPath}/{addToImage}_{file.FileName}";
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // For now, rethrowing the exception
                throw ex;
            }
        }
    }
}
