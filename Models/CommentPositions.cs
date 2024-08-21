using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Models;

public class CommentPosition
    {
        [Key]
        [ForeignKey("Kommentar")]
        public int KommentarId { get; set; }

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public string? BorderColor { get; set; }

        public virtual Kommentar? Kommentar { get; set; }
    }