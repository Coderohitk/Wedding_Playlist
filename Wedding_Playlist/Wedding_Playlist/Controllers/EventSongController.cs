using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Interfaces;
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
        /// Retrieves a list of all songs associated with an event.  
        /// This method calls the event song service to fetch all songs associated with an event.  
        /// If successful, it returns an `IEnumerable<Song>` wrapped in an `ActionResult`.  
        /// If no songs are found, it returns an empty list.  
        /// This method does not require any parameters.
        /// </summary>
        [HttpGet("EventSong")]
        public async Task<ActionResult<IEnumerable<EventSong>>> GetEventSong()
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
        /// Retrieves a specific song associated with an event.  
        /// This method takes an integer `id` as a parameter and queries the service for a matching song.  
        /// If a song is found, it returns an HTTP 200 response with the song details.  
        /// If no song matches the provided ID, it returns an HTTP 404 Not Found response.  
        /// This helps ensure that only valid song records are accessed in the system.  
        /// The method is useful for retrieving song details in a detailed view.
        /// </summary>
        [HttpGet("GetEventSongById")]
        public async Task<ActionResult<EventSong>> FindEventSong(int id)
        {
            var eventsong = await _context.EventSongs.Where(s => s.EventSongId == id).Select(s => new
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
                return NotFound(eventsong);
            }
            return Ok(eventsong);
        }

        [HttpGet("GetEventSongByEvent")]
        public async Task<ActionResult<List<EventSong>>> FindEventSongByEvent(int eventId)
        {
            var eventsong = await _context.EventSongs.Where(s => s.EventId == eventId).Select(s => new
            {
                s.SongId,
                s.EventId,
                s.EventSongId,
                SongTitle = s.Song.Title,
                SongArtist = s.Song.Artist,
                SongDescription = s.Song.Description
            }).ToListAsync();
            if (eventsong == null)
            {
                return NotFound(eventsong);
            }
            return Ok(eventsong);
        }

        [HttpGet("GetEventSongBySong")]
        public async Task<ActionResult<List<EventSong>>> FindEventSongBySong(int songId)
        {
            var eventsong = await _context.EventSongs.Where(s => s.SongId == songId).Select(s => new
            {
                s.SongId,
                s.EventId,
                s.EventSongId,
                SongTitle = s.Song.Title,
                SongArtist = s.Song.Artist,
                SongDescription = s.Song.Description
            }).ToListAsync();
            if (eventsong == null)
            {
                return NotFound(eventsong);
            }
            return Ok(eventsong);
        }
    }
}