using System;
using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;
using dotenv.net;
using Azure.Storage;
using Microsoft.Extensions.Logging; // Add this for logging
using App.Interfaces;
using Azure.Storage.Blobs.Models;

namespace App.Services
{

    public class AzureBlobStorageService : IAzureBlobStorageService
    {

        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ILogger<AzureBlobStorageService> _logger;

        public AzureBlobStorageService(ILogger<AzureBlobStorageService> logger)

        {
            _logger = logger;
            DotEnv.Load();

            var storageAccount = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_ACCOUNT");
            var containerName = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_CONTAINER");
            var accessKey = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_KEY");

            try
            {
                var credentials = new StorageSharedKeyCredential(storageAccount, accessKey);
                var blobUri = $"https://{storageAccount}.blob.core.windows.net";

                _blobServiceClient = new BlobServiceClient(new Uri(blobUri), credentials);
                _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

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

        public async Task<string> UploadImageToBlobAsync(IFormFile image)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("forschungsfragen-images");
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(image.FileName);

            try
            {
                using (var stream = image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading to Blob Storage");
                // Handle the error appropriately
            }

            return blobClient.Uri.ToString();
        }



    }
}

