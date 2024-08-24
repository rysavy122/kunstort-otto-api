using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models 
{

public class CommentPosition 
{
    [Key]
    [ForeignKey("Kommentar")]
    public int KommentarId { get; set; }

    public float XPosition { get; set; }
    public float YPosition { get; set; } 

    public string? BorderColor { get; set; }

    public virtual Kommentar? Kommentar { get; set; }
}



}
