using Microsoft.AspNetCore.Mvc;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;

namespace Wedding_Playlist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistAPIController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistAPIController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        /// <summary>
        /// Returns a list of Playlists
        /// </summary>
        /// <returns>
        /// 200 OK<br/>
        /// [{PlaylistDTO},{PlaylistDTO},..]
        /// </returns>
        /// <example>
        /// GET: api/PlaylistAPI/Playlist -> [{PlaylistDTO},{PlaylistDTO},..]
        /// </example>
        [HttpGet("Playlist")]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetAllPlaylists()
        {
            var playlists = await _playlistService.GetAllPlaylists();
            var playlistDTOs = playlists.Select(p => new PlaylistDTO
            {
                PlaylistID = p.PlaylistID,
                Name = p.Name,
                CreatedBy = p.CreatedBy
            });

            return Ok(playlistDTOs);
        }

        /// <summary>
        /// Returns a Playlist by ID
        /// </summary>
        /// <param name="id">Playlist ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// {PlaylistDTO}<br/>
        /// 404 Not Found if playlist does not exist
        /// </returns>
        /// <example>
        /// GET: api/PlaylistAPI/GetPlaylistById?id=5 -> {PlaylistDTO}
        /// </example>
        [HttpGet("GetPlaylistById")]
        public async Task<ActionResult<PlaylistDTO>> GetPlaylist(int id)
        {
            var p = await _playlistService.GetPlaylist(id);
            if (p == null) return NotFound();

            var playlistDTO = new PlaylistDTO
            {
                PlaylistID = p.PlaylistID,
                Name = p.Name,
                CreatedBy = p.CreatedBy
            };

            return Ok(playlistDTO);
        }

        /// <summary>
        /// Creates a new Playlist
        /// </summary>
        /// <param name="addplaylist">PlaylistDTO object</param>
        /// <returns>
        /// 201 Created<br/>
        /// URI to newly created resource<br/>
        /// 404 Not Found or 500 Internal Server Error on failure
        /// </returns>
        /// <example>
        /// POST: api/PlaylistAPI/AddPlaylist<br/>
        /// Body: { "name": "Reception Mix", "createdBy": "DJ Mike" }
        /// </example>
        [HttpGet("AddPlaylist")]
        public async Task<ActionResult<PlaylistDTO>> CreatePlaylist(PlaylistDTO addplaylist)
        {
            ServiceResponse response = await _playlistService.CreatePlaylist(addplaylist);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }
            addplaylist.PlaylistID = response.CreatedId;
            return Created($"api/PlaylistAPI/GetPlaylistById/{response.CreatedId}", addplaylist);
        }

        /// <summary>
        /// Updates an existing Playlist
        /// </summary>
        /// <param name="id">Playlist ID</param>
        /// <param name="updateplaylist">Updated PlaylistDTO object</param>
        /// <returns>
        /// 204 No Content<br/>
        /// 400 Bad Request if IDs do not match<br/>
        /// 404 Not Found or 500 Internal Server Error on failure
        /// </returns>
        /// <example>
        /// PUT: api/PlaylistAPI/UpdatePlaylist/5<br/>
        /// Body: { "playlistID": 5, "name": "Updated Playlist", "createdBy": "DJ John" }
        /// </example>
        [HttpPut("UpdatePlaylist/{id}")]
        public async Task<ActionResult> UpdatePlaylist(int id, PlaylistDTO updateplaylist)
        {
            if (id != updateplaylist.PlaylistID)
            {
                return BadRequest();
            }
            ServiceResponse response = await _playlistService.UpdatePlaylist(updateplaylist);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }
            return NoContent();
        }

        /// <summary>
        /// Deletes a Playlist by ID
        /// </summary>
        /// <param name="id">Playlist ID</param>
        /// <returns>
        /// 200 OK with confirmation message<br/>
        /// 404 Not Found or 500 Internal Server Error on failure
        /// </returns>
        /// <example>
        /// DELETE: api/PlaylistAPI/DeletePlaylist/5
        /// </example>
        [HttpDelete("DeletePlaylist/{id}")]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var response = await _playlistService.DeletePlaylist(id);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }
            return Ok(response.Messages);
        }
    }
}
