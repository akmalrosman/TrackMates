using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FriendsMusicTracker.Data;
using FriendsMusicTracker.Models;

namespace FriendsMusicTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistSongsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlaylistSongsController(AppDbContext context)
        {
            _context = context;
        }

        // 🔍 GET: api/playlistsongs/playlist/5 (Gets all songs inside a specific playlist)
        [HttpGet("playlist/{playlistId}")]
        public async Task<ActionResult<IEnumerable<PlaylistSong>>> GetSongsForPlaylist(int playlistId)
        {
            return await _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .ToListAsync();
        }

        // ➕ POST: api/playlistsongs (Links a song to a playlist)
        [HttpPost]
        public async Task<ActionResult<PlaylistSong>> PostPlaylistSong(PlaylistSong playlistSong)
        {
            _context.PlaylistSongs.Add(playlistSong);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSongsForPlaylist), new { playlistId = playlistSong.PlaylistId }, playlistSong);
        }

        // ❌ DELETE: api/playlistsongs/5 (Unlinks a song from a playlist)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylistSong(int id)
        {
            var playlistSong = await _context.PlaylistSongs.FindAsync(id);
            if (playlistSong == null)
            {
                return NotFound();
            }

            _context.PlaylistSongs.Remove(playlistSong);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}