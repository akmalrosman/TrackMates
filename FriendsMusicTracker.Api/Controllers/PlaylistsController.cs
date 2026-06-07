using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FriendsMusicTracker.Data;
using FriendsMusicTracker.Models;

namespace FriendsMusicTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlaylistsController(AppDbContext context)
        {
            _context = context;
        }

        // 🎵 GET: api/playlists (Loads the playlists)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists([FromQuery] string? curator)
        {
            // If a curator name is provided, only fetch theirs. Otherwise, fetch all.
            if (!string.IsNullOrEmpty(curator))
            {
                return await _context.Playlists.Where(p => p.CuratorName == curator).ToListAsync();
            }

            return await _context.Playlists.ToListAsync();
        }

        // ➕ POST: api/playlists (Adds a new playlist)
        [HttpPost]
        public async Task<ActionResult<Playlist>> PostPlaylist(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlaylists), new { id = playlist.Id }, playlist);
        }

        // ✏️ PUT: api/playlists/5 (Updates a playlist)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaylist(int id, Playlist playlist)
        {
            if (id != playlist.Id)
            {
                return BadRequest();
            }

            _context.Entry(playlist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // ❌ DELETE: api/playlists/5 (Deletes a playlist)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🎧 GET: api/playlists/5/songs (NEW: Gets all songs inside a specific playlist for the Lounge Room!)
        [HttpGet("{id}/songs")]
        public async Task<ActionResult<IEnumerable<Song>>> GetPlaylistSongs(int id)
        {
            // Look up mapped IDs through the intermediate relation table
            var mappedSongIds = await _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == id)
                .Select(ps => ps.SongId)
                .ToListAsync();

            var tracks = await _context.Songs
                .Where(song => mappedSongIds.Contains(song.Id))
                .ToListAsync();

            return Ok(tracks);
        }

        private bool PlaylistExists(int id)
        {
            return _context.Playlists.Any(e => e.Id == id);
        }
    }
}