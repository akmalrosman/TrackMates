using FriendsMusicTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FriendsMusicTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Member> Members { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
    }
}