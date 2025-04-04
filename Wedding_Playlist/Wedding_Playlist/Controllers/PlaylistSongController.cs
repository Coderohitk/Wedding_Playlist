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

        // GET: api/PlaylistSong
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllPlaylistSongs()
        {
            var playlistSongs = await _context.PlaylistSongs
                .Select(s => new
                {
                    s.PlaylistSongId,
                    s.PlaylistID,
                    PlaylistName = s.Playlist.Name,
                    s.SongID,
                    SongTitle = s.Song.Title,
                    s.Order
                }).ToListAsync();

            return Ok(playlistSongs);
        }

        // GET: api/PlaylistSong/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPlaylistSong(int id)
        {
            var playlistSong = await _context.PlaylistSongs
                .Where(ps => ps.PlaylistSongId == id)
                .Select(ps => new
                {
                    ps.PlaylistSongId,
                    ps.PlaylistID,
                    PlaylistName = ps.Playlist.Name,
                    ps.SongID,
                    SongTitle = ps.Song.Title,
                    ps.Order
                }).FirstOrDefaultAsync();

            if (playlistSong == null)
            {
                return NotFound();
            }

            return Ok(playlistSong);
        }

        // POST: api/PlaylistSong
        [HttpPost]
        public async Task<ActionResult<PlaylistSong>> CreatePlaylistSong(PlaylistSong playlistSong)
        {
            var newPlaylistSong = new PlaylistSong
            {
                PlaylistID = playlistSong.PlaylistID,
                SongID = playlistSong.SongID,
                Order = playlistSong.Order,
            };

            _context.PlaylistSongs.Add(newPlaylistSong);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlaylistSong), new { id = newPlaylistSong.PlaylistSongId }, newPlaylistSong);
        }

        // PUT: api/PlaylistSong/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PlaylistSong>> UpdatePlaylistSong(int id, PlaylistSong playlistSong)
        {
            if (id != playlistSong.PlaylistSongId)
            {
                return BadRequest();
            }

            var playlistSongToUpdate = await _context.PlaylistSongs.FindAsync(id);
            if (playlistSongToUpdate == null)
            {
                return NotFound();
            }

            playlistSongToUpdate.PlaylistID = playlistSong.PlaylistID;
            playlistSongToUpdate.SongID = playlistSong.SongID;
            playlistSongToUpdate.Order = playlistSong.Order;

            await _context.SaveChangesAsync();

            return Ok(playlistSongToUpdate);
        }

        // DELETE: api/PlaylistSong/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<PlaylistSong>> DeletePlaylistSong(int id)
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
