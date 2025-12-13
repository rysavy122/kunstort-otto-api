using System;
using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;
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
        private readonly BlobServiceClient? _client;
        private readonly ILogger<AzureBlobStorageService> _logger;

        public AzureBlobStorageService(ILogger<AzureBlobStorageService> logger, IConfiguration configuration)
        {
            _logger = logger;
            var conn =
            Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_CONNECTION_STRING") ??
            configuration["AzureStorage:ConnectionString"];

            if (string.IsNullOrWhiteSpace(conn))
            {
                _logger.LogWarning("Azure Blob Storage connection string is missing. Blob features are disabled.");
                _client = null;
                return;
            }

            _client = new BlobServiceClient(conn);
        }


        public async Task ListContainerBlobsAsync()
        {
            if (_client == null)
            {
                _logger.LogWarning("ListContainerBlobsAsync called, but Blob client is not configured.");
                return;
            }
            try
            {
                var containers = _client.GetBlobContainersAsync();

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
            if (_client == null)
            {
                _logger.LogWarning("UploadMediaAsync called, but Blob client is not configured.");
                throw new InvalidOperationException("Blob storage client is not configured.");
            }

            var containerClient = _client.GetBlobContainerClient(containerName);
            _ = await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

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

            if (_client == null)
            {
                _logger.LogWarning("DeleteMediaAsync called, but Blob client is not configured.");
                return false;
            }

            var containerClient = _client.GetBlobContainerClient(containerName);
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

