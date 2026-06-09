using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FriendsMusicTracker.Data;

namespace FriendsMusicTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats([FromQuery] string? username = null)
        {
            var totalMembers = await _context.Members.CountAsync();

            int mySongs = 0, myPlaylists = 0, myReviews = 0;

            if (!string.IsNullOrEmpty(username))
            {
                mySongs = await _context.Songs.CountAsync(s => s.AddedBy == username);
                myPlaylists = await _context.Playlists.CountAsync(p => p.CuratorName == username);
                myReviews = await _context.Reviews.CountAsync(r => r.ReviewerName == username);
            }

            return Ok(new
            {
                TotalMembers = totalMembers,
                MyTotalSongs = mySongs,
                MyTotalPlaylists = myPlaylists,
                MyTotalReviews = myReviews
            });
        }
    }
}