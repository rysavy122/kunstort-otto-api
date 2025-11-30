using System;
namespace App.Models
{
	public class Kommentar
	{
		public int Id { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
	}
}

