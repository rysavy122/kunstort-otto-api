using System;
using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;
using dotenv.net;
using Azure.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http; 
using App.Interfaces;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs.Models;
using App.Models;

namespace App.Services
{

    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<AzureBlobStorageService> _logger;

        public AzureBlobStorageService(ILogger<AzureBlobStorageService> logger, IConfiguration configuration)
        {
            _logger = logger;

            var storageAccount = configuration["AzureStorage:Account"];
            var accessKey = configuration["AzureStorage:Key"];

            try
            {
                var credentials = new StorageSharedKeyCredential(storageAccount, accessKey);
                var blobUri = $"https://{storageAccount}.blob.core.windows.net";
                _blobServiceClient = new BlobServiceClient(new Uri(blobUri), credentials);

                _logger.LogInformation("Connected to Azure Blob Storage Account: {StorageAccount}", storageAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error connecting to Azure Blob Storage");
                throw;
            }
        }

        public async Task ListContainerBlobsAsync()
        {
            try
            {
                var containers = _blobServiceClient.GetBlobContainersAsync();

                await foreach (var container in containers)
                {
                    Console.WriteLine(container.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing container blobs");
                throw;
            }
        }

        public async Task<(string Uri, FileModel FileInfo)> UploadImageToBlobAsync(IFormFile image)
        {
            return await UploadMediaAsync(image, "forschungsfragen-images");
        }

        public async Task<(string Uri, FileModel FileInfo)> UploadVideoToBlobAsync(IFormFile video)
        {
            return await UploadMediaAsync(video, "forschungsfragen-videos");
        }

        public async Task<(string Uri, FileModel FileInfo)> UploadAudioToBlobAsync(IFormFile audio)
        {
            return await UploadMediaAsync(audio, "forschungsfragen-audio");
        }

        private async Task<(string Uri, FileModel FileInfo)> UploadMediaAsync(IFormFile media, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(media.FileName);

            var fileInfo = new FileModel
            {
                FileName = media.FileName,
                FileType = media.ContentType,
                FileSize = media.Length,
                UploadDate = DateTime.UtcNow,
                BlobStorageUri = blobClient.Uri.ToString()
            };

            try
            {
                using (var stream = media.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = media.ContentType });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading to Blob Storage");
                throw;
            }

            return (blobClient.Uri.ToString(), fileInfo);
        }
        public async Task<bool> DeleteMediaAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            string containerName = GetContainerNameForFile(fileName);

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            try
            {
                await blobClient.DeleteIfExistsAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting media from Blob Storage: {FileName} in {ContainerName}", fileName, containerName);
                return false;
            }
        }

        private string GetContainerNameForFile(string fileName)
        {
            if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") || fileName.EndsWith(".png") || fileName.EndsWith(".svg") || fileName.EndsWith(".gif"))
            {
                return "forschungsfragen-images";
            }
            if (fileName.EndsWith(".mp4") || fileName.EndsWith(".mov") || fileName.EndsWith(".flv") || fileName.EndsWith(".avi") || fileName.EndsWith(".wmv"))
            {
                return "forschungsfragen-videos";
            }
            if (fileName.EndsWith(".mp3") || fileName.EndsWith(".wav") || fileName.EndsWith(".flac") || fileName.EndsWith(".aiff"))
            {
                return "forschungsfragen-audio";
            }
            return "default-container";
        }

    }
}

