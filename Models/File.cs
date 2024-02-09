using System;
namespace App.Models
{
	public class File
	{
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public DateTime? UploadDate { get; set; } = DateTime.UtcNow;
        public string FilePath { get; set; }
    }
}

