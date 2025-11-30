namespace App.Models.DTOs
{
    public class PlakatDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DrawingJson { get; set; }
        public List<StickerDto> Stickers { get; set; } = new List<StickerDto>();
    }
}
