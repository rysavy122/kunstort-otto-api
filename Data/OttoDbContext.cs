using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Data
{
    public class OttoDbContext : DbContext
    {
        public OttoDbContext(DbContextOptions<OttoDbContext> options) : base(options)
        {
        }

        public DbSet<Message>? Messages { get; set; }
        public DbSet<Forschungsfrage>? Forschungsfragen { get; set; }
        public DbSet<Kommentar>? Kommentare { get; set; }
        public DbSet<FileModel>? Files { get; set; }
         public DbSet<CommentPosition>? CommentPositions { get; set; }
         public DbSet<MediaPosition>? MediaPositions { get; set; }

    }
}
