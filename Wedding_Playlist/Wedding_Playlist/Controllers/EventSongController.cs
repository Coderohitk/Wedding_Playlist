using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Returns a list of all EventSong records with song details
        /// </summary>
        /// <returns>
        /// 200 OK<br/>
        /// [{ SongId, EventId, EventSongId, SongTitle, SongArtist, SongDescription }, ...]
        /// </returns>
        /// <example>
        /// GET: /EventSong/EventSong
        /// </example>
        [HttpGet("EventSong")]
        public async Task<ActionResult<IEnumerable<object>>> GetEventSong()
        {
            var eventsong = await _context.EventSongs.Select(s => new
            {
                s.SongId,
                s.EventId,
                s.EventSongId,
                SongTitle = s.Song.Title,
                SongArtist = s.Song.Artist,
                SongDescription = s.Song.Description
            }).ToListAsync();

            return Ok(eventsong);
        }

        /// <summary>
        /// Returns a specific EventSong by EventSongId
        /// </summary>
        /// <param name="id">EventSong ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// { SongId, EventId, EventSongId, SongTitle, SongArtist, SongDescription }<br/>
        /// 404 Not Found if not found
        /// </returns>
        /// <example>
        /// GET: /EventSong/GetEventSongById?id=5
        /// </example>
        [HttpGet("GetEventSongById")]
        public async Task<ActionResult<object>> FindEventSong(int id)
        {
            var eventsong = await _context.EventSongs
                .Where(s => s.EventSongId == id)
                .Select(s => new
                {
                    s.SongId,
                    s.EventId,
                    s.EventSongId,
                    SongTitle = s.Song.Title,
                    SongArtist = s.Song.Artist,
                    SongDescription = s.Song.Description
                }).FirstOrDefaultAsync();

            if (eventsong == null)
            {
                return NotFound();
            }

            return Ok(eventsong);
        }

        /// <summary>
        /// Returns all EventSong records for a specific EventId
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// List of EventSong objects with song details<br/>
        /// 404 Not Found if no matches
        /// </returns>
        /// <example>
        /// GET: /EventSong/GetEventSongByEvent?eventId=3
        /// </example>
        [HttpGet("GetEventSongByEvent")]
        public async Task<ActionResult<List<object>>> FindEventSongByEvent(int eventId)
        {
            var eventsong = await _context.EventSongs
                .Where(s => s.EventId == eventId)
                .Select(s => new
                {
                    s.SongId,
                    s.EventId,
                    s.EventSongId,
                    SongTitle = s.Song.Title,
                    SongArtist = s.Song.Artist,
                    SongDescription = s.Song.Description
                }).ToListAsync();

            if (eventsong == null || !eventsong.Any())
            {
                return NotFound();
            }

            return Ok(eventsong);
        }

        /// <summary>
        /// Returns all EventSong records for a specific SongId
        /// </summary>
        /// <param name="songId">Song ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// List of EventSong objects with event details<br/>
        /// 404 Not Found if no matches
        /// </returns>
        /// <example>
        /// GET: /EventSong/GetEventSongBySong?songId=7
        /// </example>
        [HttpGet("GetEventSongBySong")]
        public async Task<ActionResult<List<object>>> FindEventSongBySong(int songId)
        {
            var eventsong = await _context.EventSongs
                .Where(s => s.SongId == songId)
                .Select(s => new
                {
                    s.SongId,
                    s.EventId,
                    s.EventSongId,
                    SongTitle = s.Song.Title,
                    SongArtist = s.Song.Artist,
                    SongDescription = s.Song.Description
                }).ToListAsync();

            if (eventsong == null || !eventsong.Any())
            {
                return NotFound();
            }

            return Ok(eventsong);
        }
    }
}
