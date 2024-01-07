using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace App.Models
{
	public class Kommentar
	{
        public Kommentar()
        {
            Replies = new HashSet<Kommentar>();
        }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ParentKommentarId { get; set; }
        [JsonIgnore]
        public virtual Kommentar? ParentKommentar { get; set; }
        public virtual ICollection<Kommentar> Replies { get; set; }
    }
}

