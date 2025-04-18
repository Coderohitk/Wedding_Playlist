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

        /// <summary>
        /// Returns a list of all PlaylistSong entries
        /// </summary>
        /// <returns>
        /// 200 OK<br/>
        /// [{PlaylistSongDTO}, {PlaylistSongDTO}, ...]
        /// </returns>
        /// <example>
        /// GET: api/PlaylistSong
        /// </example>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistSongDTO>>> GetAllPlaylistSongs()
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

        /// <summary>
        /// Returns a specific PlaylistSong entry by ID
        /// </summary>
        /// <param name="id">PlaylistSong ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// {PlaylistSongDTO}<br/>
        /// 404 Not Found if not found
        /// </returns>
        /// <example>
        /// GET: api/PlaylistSong/5
        /// </example>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistSongDTO>> GetPlaylistSong(int id)
        {
            var playlistSong = await _context.PlaylistSongs
                .Where(ps => ps.PlaylistSongId == id)
                .FirstOrDefaultAsync();

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

        /// <summary>
        /// Creates a new PlaylistSong entry
        /// </summary>
        /// <param name="playlistSongDTO">PlaylistSongDTO object</param>
        /// <returns>
        /// 201 Created<br/>
        /// URI to newly created resource
        /// </returns>
        /// <example>
        /// POST: api/PlaylistSong<br/>
        /// Body: { "playlistID": 1, "songID": 2, "order": 3 }
        /// </example>
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

        /// <summary>
        /// Updates an existing PlaylistSong entry
        /// </summary>
        /// <param name="id">PlaylistSong ID</param>
        /// <param name="playlistSongDTO">Updated PlaylistSongDTO object</param>
        /// <returns>
        /// 200 OK with updated object<br/>
        /// 400 Bad Request if ID mismatch<br/>
        /// 404 Not Found if not found
        /// </returns>
        /// <example>
        /// PUT: api/PlaylistSong/5<br/>
        /// Body: { "playlistSongId": 5, "playlistID": 1, "songID": 2, "order": 3 }
        /// </example>
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

        /// <summary>
        /// Deletes a PlaylistSong entry by ID
        /// </summary>
        /// <param name="id">PlaylistSong ID</param>
        /// <returns>
        /// 200 OK with deleted PlaylistSong object<br/>
        /// 404 Not Found if not found
        /// </returns>
        /// <example>
        /// DELETE: api/PlaylistSong/4
        /// </example>
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
