namespace App.Models.DTOs
{
    public class StickerDto
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string BlobStorageUri { get; set; }

        public int PlakatId { get; set; }
    }
}
