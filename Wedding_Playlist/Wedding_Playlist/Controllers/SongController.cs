using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;

namespace WeddingPlaylist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Songs
        /// </summary>
        /// <returns>
        /// 200 OK<br/>
        /// [{SongDTO},{SongDTO},..]
        /// </returns>
        /// <example>
        /// GET: api/Song -> [{SongDTO},{SongDTO},..]
        /// </example>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongDTO>>> GetSongs()
        {
            var songs = await _context.Songs
                .Select(s => new SongDTO
                {
                    SongId = s.SongId,
                    Title = s.Title,
                    Artist = s.Artist,
                    Genre = s.Genre,
                    Description = s.Description
                })
                .ToListAsync();

            return Ok(songs);
        }

        /// <summary>
        /// Returns a Song by ID
        /// </summary>
        /// <param name="id">Song ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// {SongDTO}<br/>
        /// 404 Not Found if song does not exist
        /// </returns>
        /// <example>
        /// GET: api/Song/3 -> {SongDTO}
        /// </example>
        [HttpGet("{id}")]
        public async Task<ActionResult<SongDTO>> GetSong([FromRoute] int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            var songDTO = new SongDTO
            {
                SongId = song.SongId,
                Title = song.Title,
                Artist = song.Artist,
                Genre = song.Genre,
                Description = song.Description
            };

            return Ok(songDTO);
        }

        /// <summary>
        /// Creates a new Song
        /// </summary>
        /// <param name="songDTO">SongDTO object</param>
        /// <returns>
        /// 201 Created<br/>
        /// URI to newly created resource<br/>
        /// 400 Bad Request if data is invalid
        /// </returns>
        /// <example>
        /// POST: api/Song<br/>
        /// Body: { "title": "Perfect", "artist": "Ed Sheeran", "genre": "Pop", "description": "Wedding song" }
        /// </example>
        [HttpPost]
        public async Task<ActionResult<SongDTO>> CreateSong([FromBody] SongDTO songDTO)
        {
            if (songDTO == null)
            {
                return BadRequest("Invalid song data.");
            }

            var song = new Song
            {
                Title = songDTO.Title,
                Artist = songDTO.Artist,
                Genre = songDTO.Genre,
                Description = songDTO.Description
            };

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            songDTO.SongId = song.SongId;
            return CreatedAtAction(nameof(GetSong), new { id = song.SongId }, songDTO);
        }

        /// <summary>
        /// Updates an existing Song
        /// </summary>
        /// <param name="id">Song ID</param>
        /// <param name="songDTO">Updated SongDTO object</param>
        /// <returns>
        /// 204 No Content<br/>
        /// 400 Bad Request if ID mismatch<br/>
        /// 404 Not Found if song does not exist
        /// </returns>
        /// <example>
        /// PUT: api/Song/3<br/>
        /// Body: { "songId": 3, "title": "Updated Title", "artist": "New Artist", "genre": "Jazz", "description": "Updated info" }
        /// </example>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong([FromRoute] int id, [FromBody] SongDTO songDTO)
        {
            if (id != songDTO.SongId)
            {
                return BadRequest("Song ID mismatch.");
            }

            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            song.Title = songDTO.Title;
            song.Artist = songDTO.Artist;
            song.Genre = songDTO.Genre;
            song.Description = songDTO.Description;

            _context.Entry(song).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a Song by ID
        /// </summary>
        /// <param name="id">Song ID</param>
        /// <returns>
        /// 204 No Content<br/>
        /// 404 Not Found if song does not exist
        /// </returns>
        /// <example>
        /// DELETE: api/Song/3
        /// </example>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong([FromRoute] int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
