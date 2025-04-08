using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;

namespace WeddingPlaylist.Controllers
{
    [Route("api/[controller]")] // Base route: api/song
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/song - Retrieve all songs
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

        // ✅ GET: api/song/{id} - Retrieve a song by ID
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

        // ✅ POST: api/song - Create a new song
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

            songDTO.SongId = song.SongId; // Assign the newly created ID
            return CreatedAtAction(nameof(GetSong), new { id = song.SongId }, songDTO);
        }

        // ✅ PUT: api/song/{id} - Update song details
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

            // Update the song details
            song.Title = songDTO.Title;
            song.Artist = songDTO.Artist;
            song.Genre = songDTO.Genre;
            song.Description = songDTO.Description;

            _context.Entry(song).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/song/{id} - Delete a song
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
