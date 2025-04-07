using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;

namespace Wedding_Playlist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestSongRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GuestSongRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all guest song requests.
        /// Returns
        /// List of GuestSongRequestDTO objects.
        /// This method is an asynchronous operation that calls the database to fetch all guest song requests.  
        /// It then maps each guest song request to a GuestSongRequestDTO object and returns the list as an ActionResult.  
        /// If there are no guest song requests, it returns an empty list.  
        /// This method is marked as a GET request and has no parameters.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuestSongRequestDTO>>> GetAllGuestSongRequests()
        {
            var guestSongRequests = await _context.GuestSongRequests
                .Select(s => new GuestSongRequestDTO
                {
                    RequestID = s.RequestID,
                    EventID = s.EventID,
                    GuestID = s.GuestID,
                    SongID = s.SongID,
                    Status = s.Status
                }).ToListAsync();
            return Ok(guestSongRequests);
        }

        /// <summary>
        /// Gets a specific guest song request by ID.
        /// Returns
        /// An GuestSongRequestDTO object if found; otherwise, 404 NotFound.
        /// This method takes an integer `id` as a parameter and queries the database for a guest song request with the matching ID.  
        /// If a guest song request is found, it returns an HTTP 200 response with the guest song request details.  
        /// If no guest song request matches the provided ID, it returns an HTTP 404 Not Found response.  
        /// This helps ensure that only valid guest song requests are accessed in the system.  
        /// The method is useful for retrieving guest song request details in a detailed view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GuestSongRequestDTO>> GetGuestSongRequest(int id)
        {
            var guestSongRequest = await _context.GuestSongRequests
                .Where(s => s.RequestID == id).FirstOrDefaultAsync();
            if (guestSongRequest == null)
            {
                return NotFound();
            }
            var guestSongRequestDTO = new GuestSongRequestDTO
            {
                RequestID = guestSongRequest.RequestID,
                EventID = guestSongRequest.EventID,
                GuestID = guestSongRequest.GuestID,
                SongID = guestSongRequest.SongID,
                Status = guestSongRequest.Status
            };
            return Ok(guestSongRequestDTO);
        }
        /// <summary>
        /// Adds a new guest song request to the database.
        /// Returns
        /// Status with CreatedId or error message.
        /// This method takes a `GuestSongRequestDTO` object as a parameter and creates a new guest song request in the database.  
        /// It then maps the new guest song request to a `GuestSongRequestDTO` object and returns the created ID as an ActionResult.  
        /// If the guest song request creation fails, it returns an HTTP 500 Internal Server Error response.  
        /// This method is marked as a POST request and has a [FromBody] attribute, which tells the controller to bind the request body to the `GuestSongRequestDTO` object.
        /// </summary>
        /// <param name="guestSongRequestDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<GuestSongRequestDTO>> CreateGuestSongRequest([FromBody] GuestSongRequestDTO guestSongRequestDTO)
        {
            if (guestSongRequestDTO == null)
            {
                return BadRequest();
            }
            var newGuestSongRequest = new GuestSongRequest
            {
                EventID = guestSongRequestDTO.EventID,
                GuestID = guestSongRequestDTO.GuestID,
                SongID = guestSongRequestDTO.SongID,
                Status = guestSongRequestDTO.Status

            };
            _context.GuestSongRequests.Add(newGuestSongRequest);
            await _context.SaveChangesAsync();
            guestSongRequestDTO.RequestID = newGuestSongRequest.RequestID;
            return CreatedAtAction(nameof(GetGuestSongRequest), new { id = newGuestSongRequest.RequestID }, guestSongRequestDTO);
        }
        /// <summary>
        /// Updates an existing guest song request in the database.
        /// Returns
        /// Status with error message if not found.
        /// This method takes an integer `id` as a parameter and an updated `GuestSongRequestDTO` object in the request body.  
        /// It then updates the guest song request in the database with the new details.  
        /// If the ID in the URL does not match the one in the object, it returns an HTTP 400 Bad Request response.  
        /// If the guest song request does not exist, an HTTP 404 Not Found response is returned.  
        /// On successful update, the method returns an HTTP 204 No Content response.  
        /// This ensures that modifications to guest song requests are properly validated and processed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guestSongRequestDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<GuestSongRequestDTO>> UpdateGuestSongRequest([FromRoute] int id, [FromBody] GuestSongRequestDTO guestSongRequestDTO)
        {
            if (id != guestSongRequestDTO.RequestID)
            {
                return BadRequest();
            }
            var guestSongRequestToUpdate = await _context.GuestSongRequests.FindAsync(id);
            if (guestSongRequestToUpdate == null)
            {
                return NotFound();
            }

            guestSongRequestToUpdate.EventID = guestSongRequestDTO.EventID;
            guestSongRequestToUpdate.GuestID = guestSongRequestDTO.GuestID;
            guestSongRequestToUpdate.SongID = guestSongRequestDTO.SongID;
            guestSongRequestToUpdate.Status = guestSongRequestDTO.Status;

            await _context.SaveChangesAsync();

            return Ok(guestSongRequestToUpdate);
        }
        /// <summary>
        /// Deletes a guest song request from the database.
        /// Returns
        /// Status with error message if not found.  
        /// This method takes an integer `id` as a parameter and attempts to remove the corresponding guest song request record.  
        /// If the guest song request exists, it is deleted, and an HTTP 200 OK response with a confirmation message is returned.  
        /// If the guest song request does not exist, an HTTP 404 Not Found response is returned.  
        /// Any unexpected issues, such as database errors, result in an HTTP 500 Internal Server Error response.  
        /// This method ensures proper deletion while handling errors gracefully.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<GuestSongRequestDTO>> DeleteGuestSongRequest([FromRoute] int id)
        {
            var guestSongRequest = await _context.GuestSongRequests.FindAsync(id);
            if (guestSongRequest == null)
            {
                return NotFound();
            }

            _context.GuestSongRequests.Remove(guestSongRequest);
            await _context.SaveChangesAsync();

            return Ok(guestSongRequest);
        }
    }
}