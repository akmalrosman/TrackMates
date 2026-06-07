using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FriendsMusicTracker.Data;
using FriendsMusicTracker.Models;

namespace FriendsMusicTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SongsController(AppDbContext context)
        {
            _context = context;
        }

        // 🎧 GET: api/songs?addedBy=John
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs([FromQuery] string? addedBy)
        {
            // If a username is provided, only fetch their specific songs
            if (!string.IsNullOrEmpty(addedBy))
            {
                var userSongs = await _context.Songs
                    .Where(s => s.AddedBy == addedBy)
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(userSongs);
            }

            // Otherwise, fetch ALL songs in the database (Needed for the Reviews page!)
            var allSongs = await _context.Songs
                .AsNoTracking()
                .ToListAsync();

            return Ok(allSongs);
        }

        // ➕ POST: api/songs
        [HttpPost]
        public async Task<ActionResult<Song>> PostSong([FromBody] Song song)
        {
            if (song == null) return BadRequest();

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSongs), new { addedBy = song.AddedBy }, song);
        }

        // ✏️ PUT: api/songs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSong(int id, [FromBody] Song song)
        {
            if (id != song.Id) return BadRequest("ID mismatch");

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Songs.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // ❌ DELETE: api/songs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return NotFound();

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}