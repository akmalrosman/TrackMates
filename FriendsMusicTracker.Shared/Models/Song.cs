namespace FriendsMusicTracker.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string YoutubeUrl { get; set; } = string.Empty;
        public string AddedBy { get; set; } = "Unknown";
    }
}