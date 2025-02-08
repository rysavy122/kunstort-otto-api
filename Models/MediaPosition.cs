using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class MediaPosition
    {
        [Key]
        [ForeignKey("FileModel")]
        public int FileModelId { get; set; }
        public float XPosition { get; set; }
        public float YPosition { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Rotation { get; set; }
        public virtual FileModel? FileModel { get; set; }
    }
}
