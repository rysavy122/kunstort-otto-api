using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class Plakat
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string DrawingJson { get; set; }  // To store the drawing as JSON
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for stickers
        public List<Sticker> Stickers { get; set; } = new List<Sticker>();
    }
}
