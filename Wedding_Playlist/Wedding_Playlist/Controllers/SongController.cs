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

        /// <summary>
        /// Gets all songs.
        /// Returns
        /// List of SongDTO objects.
        /// This method is an asynchronous operation that calls the database to fetch all songs.  
        /// It then maps each song to a SongDTO object and returns the list as an ActionResult.  
        /// If there are no songs, it returns an empty list.  
        /// This method is marked as a GET request and has no parameters.
        /// </summary>
        /// <returns></returns>
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
        /// Gets a specific song by ID.
        /// Returns
        /// An SongDTO object if found; otherwise, 404 NotFound.
        /// This method takes an integer `id` as a parameter and queries the database for a song with the matching ID.  
        /// If a song is found, it returns an HTTP 200 response with the song details.  
        /// If no song matches the provided ID, it returns an HTTP 404 Not Found response.  
        /// This helps ensure that only valid songs are accessed in the system.  
        /// The method is useful for retrieving song details in a detailed view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Adds a new song to the database.
        /// Returns
        /// Status with CreatedId or error message.
        /// </summary>
        /// <param name="songDTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates an existing song in the database.
        /// Returns
        /// Status with error message if not found.
        /// This method takes an integer `id` as a parameter and an updated `SongDTO` object in the request body.  
        /// It then updates the song in the database with the new details.  
        /// If the ID in the URL does not match the one in the object, it returns an HTTP 400 Bad Request response.  
        /// If the song does not exist, an HTTP 404 Not Found response is returned.  
        /// On successful update, the method returns an HTTP 204 No Content response.  
        /// This ensures that modifications to songs are properly validated and processed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="songDTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a song from the database.
        /// Returns
        /// Status with error message if not found.  
        /// This method takes an integer `id` as a parameter and attempts to remove the corresponding song record.  
        /// If the song exists, it is deleted, and an HTTP 200 OK response with a confirmation message is returned.  
        /// If the song does not exist, an HTTP 404 Not Found response is returned.  
        /// Any unexpected issues, such as database errors, result in an HTTP 500 Internal Server Error response.  
        /// This method ensures proper deletion while handling errors gracefully.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
