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
        /// Gets all playlist songs.
        /// Returns
        /// List of PlaylistSongDTO objects.
        /// This method is an asynchronous operation that calls the database to fetch all playlist songs.  
        /// It then maps each playlist song to a PlaylistSongDTO object and returns the list as an ActionResult.  
        /// If there are no playlist songs, it returns an empty list.  
        /// This method is marked as a GET request and has no parameters.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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

        /// <summary>
        /// Gets a specific playlist song by ID.
        /// Returns
        /// An PlaylistSongDTO object if found; otherwise, 404 NotFound.
        /// This method takes an integer `id` as a parameter and queries the database for a playlist song with the matching ID.  
        /// If a playlist song is found, it returns an HTTP 200 response with the playlist song details.  
        /// If no playlist song matches the provided ID, it returns an HTTP 404 Not Found response.  
        /// This helps ensure that only valid playlist songs are accessed in the system.  
        /// The method is useful for retrieving playlist song details in a detailed view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Adds a new playlist song to the database.
        /// Returns
        /// Status with CreatedId or error message.
        /// </summary>
        /// <param name="playlistSongDTO"></param>
        /// <returns></returns>
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
        /// Updates an existing playlist song in the database.
        /// Returns
        /// Status with error message if not found.
        /// This method takes an integer `id` as a parameter and an updated `PlaylistSongDTO` object in the request body.  
        /// It then updates the playlist song in the database with the new details.  
        /// If the ID in the URL does not match the one in the object, it returns an HTTP 400 Bad Request response.  
        /// If the playlist song does not exist, an HTTP 404 Not Found response is returned.  
        /// On successful update, the method returns an HTTP 204 No Content response.  
        /// This ensures that modifications to playlist songs are properly validated and processed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="playlistSongDTO"></param>
        /// <returns></returns>
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
        /// Deletes a playlist song from the database.
        /// Returns
        /// Status with error message if not found.  
        /// This method takes an integer `id` as a parameter and attempts to remove the corresponding playlist song record.  
        /// If the playlist song exists, it is deleted, and an HTTP 200 OK response with a confirmation message is returned.  
        /// If the playlist song does not exist, an HTTP 404 Not Found response is returned.  
        /// Any unexpected issues, such as database errors, result in an HTTP 500 Internal Server Error response.  
        /// This method ensures proper deletion while handling errors gracefully.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

