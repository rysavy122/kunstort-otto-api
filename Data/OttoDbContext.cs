using System;
using App.Models;
using Microsoft.EntityFrameworkCore;

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

    }
}

