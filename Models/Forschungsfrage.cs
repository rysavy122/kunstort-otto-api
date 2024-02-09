using System;
namespace App.Models
{
	public class Forschungsfrage
	{
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ImagePath { get; set; }
    }
}

