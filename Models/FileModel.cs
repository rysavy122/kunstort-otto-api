using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.Models
{
    [Table("files")]
    public class FileModel
	{
        public FileModel()
        {
            MediaPositions = new HashSet<MediaPosition>();

        }
        [Key]
        public int ID { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public long FileSize { get; set; }
        public DateTime? UploadDate { get; set; } = DateTime.UtcNow;
        public string? BlobStorageUri { get; set; }

        public int? ForschungsfrageId { get; set; }
        public Forschungsfrage? Forschungsfrage { get; set; }

        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<MediaPosition> MediaPositions { get; set; }

    }
}

