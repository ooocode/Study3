using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Database.VideoDb
{
    public class VideoDbContext : DbContext
    {
        public VideoDbContext(DbContextOptions<VideoDbContext> options)
            :base(options)
        {

        }


        public DbSet<VideoType> VideoTypes { get; set; }

        public DbSet<Video> Videos { get; set; }


        public DbSet<VideoUrl> VideoUrls { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<VideoType>().HasIndex(e => e.Name).IsUnique();


            modelBuilder.Entity<Video>().HasIndex(e => new { e.VideoTypeId });
            modelBuilder.Entity<Video>().HasIndex(e =>new { e.LastUpdateTime ,e.Year});
            modelBuilder.Entity<Video>().HasIndex(e => new { e.Name, e.Actor, e.Director });


            modelBuilder.Entity<VideoUrl>().HasKey(e => new { e.VideoId, e.Flag });
        }
    }
}
