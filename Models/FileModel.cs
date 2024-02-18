using System;
namespace App.Models
{
	public class FileModel
	{
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public DateTime? UploadDate { get; set; } = DateTime.UtcNow;
        public string BlobStorageUri { get; set; }
    }
}

