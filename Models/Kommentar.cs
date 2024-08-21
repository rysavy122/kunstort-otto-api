using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace App.Models
{
	public class Kommentar
	{
        public Kommentar()
        {
            Replies = new HashSet<Kommentar>();
            CommentPositions = new HashSet<CommentPosition>();

        }
        
        [Key]
        public int Id { get; set; }  // Primary key
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ParentKommentarId { get; set; }
        [JsonIgnore]
        public virtual Kommentar? ParentKommentar { get; set; }
        public virtual ICollection<Kommentar> Replies { get; set; }
        public virtual ICollection<CommentPosition> CommentPositions { get; set; }

    }
}

