using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
namespace Wedding_Playlist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventSongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EventSongController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventSong>>> GetAllEventSongs()
        {
            var eventSongs = await _context.EventSongs.Select(s => new
            {
                s.EventSongId,
                s.EventId,
                EventName = s.Event.Name,  // Fetch Event Name
                s.SongId,
                SongTitle = s.Song.Title,  // Fetch Song Title

            }).ToListAsync();
            return Ok(eventSongs);
        }
        [HttpGet]
        public async Task<ActionResult> GetEventSong(int id)
        {
            var eventSong = await _context.EventSongs.Where(es => es.EventSongId == id).Select(es => new
            {
                es.EventSongId,
                es.EventId,
                EventName = es.Event.Name,  // Fetch Event Name
                es.SongId,
                SongTitle = es.Song.Title,  // Fetch Song Title

            }).FirstOrDefaultAsync();
            if (eventSong == null)
            {
                return NotFound();
            }
            return Ok(eventSong);

        }
    }
}