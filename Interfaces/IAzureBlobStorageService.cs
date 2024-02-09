using System;
using Microsoft.AspNetCore.Http; // Required for IFormFile
namespace App.Interfaces


{
	public interface IAzureBlobStorageService
	{
        Task ListContainerBlobsAsync();
        Task<string> UploadImageToBlobAsync(IFormFile image); // Updated signature
    }
}

