using System;
using App.Models;
using Microsoft.AspNetCore.Http;
namespace App.Interfaces


{
	public interface IAzureBlobStorageService
	{
        Task ListContainerBlobsAsync();
        Task<(string Uri, FileModel FileInfo)> UploadImageToBlobAsync(IFormFile image);
        Task<(string Uri, FileModel FileInfo)> UploadVideoToBlobAsync(IFormFile video);
        Task<(string Uri, FileModel FileInfo)> UploadAudioToBlobAsync(IFormFile audio);
        Task<bool> DeleteMediaAsync(string fileName);

    }
}

