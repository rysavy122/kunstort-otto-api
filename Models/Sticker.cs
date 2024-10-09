using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class Sticker
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public string BlobStorageUri { get; set; }

        public int PlakatId { get; set; }
        public Plakat Plakat { get; set; }  // Navigation property to the associated plakat
    }
}
