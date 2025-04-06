using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;

namespace Wedding_Playlist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistSongController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlaylistSongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PlaylistSong [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetAllPlaylistSongs()
        {
            var playlistSongs = await _context.PlaylistSongs
                .Select(s => new PlaylistSongDTO
                {
                    PlaylistSongId = s.PlaylistSongId,
                    PlaylistID = s.PlaylistID,
                    SongID = s.SongID,
                    Order = s.Order
                }).ToListAsync();
            return Ok(playlistSongs);
        }

        // GET: api/PlaylistSong/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistSongDTO>> GetPlaylistSong(int id)
        {
            var playlistSong = await _context.PlaylistSongs
                .Where(ps => ps.PlaylistSongId == id).FirstOrDefaultAsync();
            if (playlistSong == null)
            {
                return NotFound();
            }
            var playlistSongDTO = new PlaylistSongDTO
            {
                PlaylistSongId = playlistSong.PlaylistSongId,
                PlaylistID = playlistSong.PlaylistID,
                SongID = playlistSong.SongID,
                Order = playlistSong.Order
            };
            return Ok(playlistSongDTO);
        }
        [HttpPost]
        public async Task<ActionResult<PlaylistSongDTO>> CreatePlaylistSong([FromBody] PlaylistSongDTO playlistSongDTO)
        {
            if (playlistSongDTO == null)
            {
                return BadRequest();
            }
            var newPlaylistSong = new PlaylistSong
            {
                PlaylistID = playlistSongDTO.PlaylistID,
                SongID = playlistSongDTO.SongID,
                Order = playlistSongDTO.Order

            };
            _context.PlaylistSongs.Add(newPlaylistSong);
            await _context.SaveChangesAsync();
            playlistSongDTO.PlaylistSongId = newPlaylistSong.PlaylistSongId;
            return CreatedAtAction(nameof(GetPlaylistSong), new { id = newPlaylistSong.PlaylistSongId }, playlistSongDTO);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<PlaylistSongDTO>> UpdatePlaylistSong([FromRoute] int id, [FromBody] PlaylistSongDTO playlistSongDTO)
        {
            if (id != playlistSongDTO.PlaylistSongId)
            {
                return BadRequest();
            }
            var playlistSongToUpdate = await _context.PlaylistSongs.FindAsync(id);
            if (playlistSongToUpdate == null)
            {
                return NotFound();
            }

            playlistSongToUpdate.PlaylistID = playlistSongDTO.PlaylistID;
            playlistSongToUpdate.SongID = playlistSongDTO.SongID;
            playlistSongToUpdate.Order = playlistSongDTO.Order;

            await _context.SaveChangesAsync();

            return Ok(playlistSongToUpdate);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<PlaylistSongDTO>> DeletePlaylistSong([FromRoute] int id)
        {
            var playlistSong = await _context.PlaylistSongs.FindAsync(id);
            if (playlistSong == null)
            {
                return NotFound();
            }

            _context.PlaylistSongs.Remove(playlistSong);
            await _context.SaveChangesAsync();

            return Ok(playlistSong);
        }
    }
}

