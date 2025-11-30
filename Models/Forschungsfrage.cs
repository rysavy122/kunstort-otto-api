using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    [Table("forschungsfragen")]
    public class Forschungsfrage
	{
        public int ID { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ImagePath { get; set; }

        public ICollection<FileModel>? Files { get; set; }
        public string BackgroundColor { get; set; } = "#FFFFFF"; // Default color is white

    }
}

