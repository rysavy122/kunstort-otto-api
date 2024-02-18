using System;
using App.Models;
using Microsoft.AspNetCore.Http;
namespace App.Interfaces


{
	public interface IAzureBlobStorageService
	{
        Task ListContainerBlobsAsync();
        Task<(string Uri, FileModel FileInfo)> UploadImageToBlobAsync(IFormFile image);
    }
}

