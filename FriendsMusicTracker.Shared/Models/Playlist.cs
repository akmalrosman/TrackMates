using System;

namespace FriendsMusicTracker.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string PlaylistName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CuratorName { get; set; } = "Unknown DJ";
    }
}