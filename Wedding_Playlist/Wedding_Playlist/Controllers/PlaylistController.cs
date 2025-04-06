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
        /// Retrieves a list of all playlists stored in the system.  
        /// This method asynchronously calls the playlist service to fetch all available playlists.  
        /// It returns an `IEnumerable<Playlist>` wrapped in an `ActionResult`.  
        /// If successful, it responds with an HTTP 200 status along with the list of playlists.  
        /// If there are no playlists, it still returns an empty list rather than an error.  
        /// This method does not require any parameters.
        /// </summary>
        [HttpGet("Playlist")]
        public async Task<ActionResult<IEnumerable<Playlist>>> GetAllPlaylists()
        {
            IEnumerable<Playlist> playlist = await _playlistService.GetAllPlaylists();
            return Ok(playlist);
        }
        /// <summary>
        /// Fetches a specific playlist by their unique ID.  
        /// The method takes an integer `id` as a parameter and queries the service for a matching playlist.  
        /// If a playlist is found, it returns an HTTP 200 response with the playlist details.  
        /// If no playlist matches the provided ID, it returns an HTTP 404 Not Found response.  
        /// This helps ensure that only valid playlist records are accessed in the system.  
        /// The method is useful for retrieving playlist details in a detailed view.
        /// </summary>
        [HttpGet("GetPlaylistById")]
        public async Task<ActionResult<Playlist>> GetPlaylist(int id)
        {
            var playlist = await _playlistService.GetPlaylist(id);
            if (playlist == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(playlist);
            }
        }
        /// <summary>
        /// Retrieves a list of playlists based on their assigned category.  
        /// The method takes a `PlaylistCategory` enum as a parameter to filter playlists accordingly.  
        /// It queries the playlist service and returns a list of matching playlists, wrapped in an HTTP 200 response.  
        /// If no playlists are found in the specified category, an HTTP 404 Not Found response is returned.  
        /// This method helps in segmenting playlists into different categories for better organization.  
        /// Useful for event planners who need to sort playlists based on roles like VIPs or general attendees.
        /// </summary>
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
            return Created($"api/Playlist/GetPlaylistById/{response.CreatedId}", addplaylist);
        }
        /// <summary>
        /// Updates an existing playlist's details in the system.  
        /// It requires the playlist ID as a URL parameter and the updated `Playlist` object in the request body.  
        /// If the ID in the URL does not match the one in the object, it returns an HTTP 400 Bad Request response.  
        /// If the playlist does not exist, an HTTP 404 Not Found response is returned.  
        /// On successful update, the method returns an HTTP 204 No Content response.  
        /// This ensures that modifications to playlist details are properly validated and processed.
        /// </summary>
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
        /// Deletes a playlist from the system based on their unique ID.  
        /// It accepts an integer `id` as a parameter and attempts to remove the corresponding playlist record.  
        /// If the playlist exists, it is deleted, and an HTTP 200 OK response with a confirmation message is returned.  
        /// If the playlist does not exist, an HTTP 404 Not Found response is returned.  
        /// Any unexpected issues, such as database errors, result in an HTTP 500 Internal Server Error response.  
        /// This method ensures proper deletion while handling errors gracefully.
        /// </summary>
        [HttpDelete("DeletePlaylist/{id}")]
        public async Task<ActionResult<Playlist>> DeletePlaylist(int id)
        {
            ServiceResponse response = await _playlistService.DeletePlaylist(id);
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