using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class MediaPosition
    {
        [Key]
        [ForeignKey("FileModel")]
        public int FileModelId { get; set; }

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public string? BorderColor { get; set; }

        public virtual FileModel? FileModel { get; set; }
    }
}
