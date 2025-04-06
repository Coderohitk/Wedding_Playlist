using Microsoft.AspNetCore.Mvc;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;

namespace MilestoneManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestAPIController : ControllerBase
    {
        private readonly IGuestService _guestService;
        public GuestAPIController(IGuestService guestService)
        {
            _guestService = guestService;
        }
        /// <summary>
        /// Retrieves a list of all guests stored in the system.  
        /// This method asynchronously calls the guest service to fetch all available guests.  
        /// It returns an `IEnumerable<Guest>` wrapped in an `ActionResult`.  
        /// If successful, it responds with an HTTP 200 status along with the list of guests.  
        /// If there are no guests, it still returns an empty list rather than an error.  
        /// This method does not require any parameters.
        /// </summary>
        [HttpGet("Guest")]
        public async Task<ActionResult<IEnumerable<Guest>>> GetGuest()
        {
            IEnumerable<Guest> guest = await _guestService.GetGuests();
            return Ok(guest);
        }

        /// <summary>
        /// Fetches a specific guest by their unique ID.  
        /// The method takes an integer `id` as a parameter and queries the service for a matching guest.  
        /// If a guest is found, it returns an HTTP 200 response with the guest details.  
        /// If no guest matches the provided ID, it returns an HTTP 404 Not Found response.  
        /// This helps ensure that only valid guest records are accessed in the system.  
        /// The method is useful for retrieving guest details in a detailed view.
        /// </summary>
        [HttpGet("GetGuestById")]
        public async Task<ActionResult<Guest>> FindGuest(int id)
        {
            var guest = await _guestService.GetGuestById(id);
            if (guest == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(guest);
            }
        }

        /// <summary>
        /// Updates an existing guest's details in the system.  
        /// It requires the guest ID as a URL parameter and the updated `Guest` object in the request body.  
        /// If the ID in the URL does not match the one in the object, it returns an HTTP 400 Bad Request response.  
        /// If the guest does not exist, an HTTP 404 Not Found response is returned.  
        /// On successful update, the method returns an HTTP 204 No Content response.  
        /// This ensures that modifications to guest details are properly validated and processed.
        /// </summary>
        [HttpPut("UpdateGuest/{id}")]
        public async Task<ActionResult> UpdateGuest(int id, GuestDTO updateguest)
        {
            if (id != updateguest.GuestId)
            {
                return BadRequest();
            }
            ServiceResponse response = await _guestService.UpdateGuest(updateguest);
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
        /// Deletes a guest from the system based on their unique ID.  
        /// It accepts an integer `id` as a parameter and attempts to remove the corresponding guest record.  
        /// If the guest exists, it is deleted, and an HTTP 200 OK response with a confirmation message is returned.  
        /// If the guest does not exist, an HTTP 404 Not Found response is returned.  
        /// Any unexpected issues, such as database errors, result in an HTTP 500 Internal Server Error response.  
        /// This method ensures proper deletion while handling errors gracefully.
        /// </summary>
        [HttpDelete("DeleteGuest/{id}")]
        public async Task<ActionResult<Guest>> DeleteGuest(int id)
        {
            ServiceResponse response = await _guestService.DeleteGuest(id);
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