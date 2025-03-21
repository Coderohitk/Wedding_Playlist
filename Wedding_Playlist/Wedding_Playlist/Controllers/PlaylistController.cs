using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Models;
using Wedding_Playlist.Data;

namespace WeddingPlaylist.Controllers
{
    [Route("api/[controller]")] // Base route: api/playlist
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlaylistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/playlist - Retrieve all playlists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetPlaylists()
        {
            var playlists = await _context.Playlists
                .Select(p => new PlaylistDTO
                {
                    PlaylistID = p.PlaylistID,
                    Name = p.Name,
                    CreatedBy = p.CreatedBy
                })
                .ToListAsync();

            return Ok(playlists);
        }

        // ✅ GET: api/playlist/{id} - Retrieve a playlist by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDTO>> GetPlaylist([FromRoute] int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            var playlistDTO = new PlaylistDTO
            {
                PlaylistID = playlist.PlaylistID,
                Name = playlist.Name,
                CreatedBy = playlist.CreatedBy
            };

            return Ok(playlistDTO);
        }

        // ✅ POST: api/playlist - Create a new playlist
        [HttpPost]
        public async Task<ActionResult<PlaylistDTO>> CreatePlaylist([FromBody] PlaylistDTO playlistDTO)
        {
            if (playlistDTO == null)
            {
                return BadRequest("Invalid playlist data.");
            }

            var playlist = new Playlist
            {
                Name = playlistDTO.Name,
                CreatedBy = playlistDTO.CreatedBy
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            playlistDTO.PlaylistID = playlist.PlaylistID; // Assign new ID
            return CreatedAtAction(nameof(GetPlaylist), new { id = playlist.PlaylistID }, playlistDTO);
        }

        // ✅ PUT: api/playlist/{id} - Update playlist details
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlaylist([FromRoute] int id, [FromBody] PlaylistDTO playlistDTO)
        {
            if (id != playlistDTO.PlaylistID)
            {
                return BadRequest("Playlist ID mismatch.");
            }

            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }

            // Update playlist details
            playlist.Name = playlistDTO.Name;
            playlist.CreatedBy = playlistDTO.CreatedBy;

            _context.Entry(playlist).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/playlist/{id} - Delete a playlist
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist([FromRoute] int id)
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
    }
}
