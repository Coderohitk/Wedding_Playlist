using Microsoft.AspNetCore.Mvc;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;

namespace Wedding_Playlist.Controllers
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
        /// Returns a list of all Guests
        /// </summary>
        /// <returns>
        /// 200 OK<br/>
        /// [{GuestDTO},{GuestDTO},..]
        /// </returns>
        /// <example>
        /// GET: api/GuestAPI/Guest
        /// </example>
        [HttpGet("Guest")]
        public async Task<ActionResult<IEnumerable<GuestDTO>>> GetGuest()
        {
            var guests = await _guestService.GetGuests();
            var guestDTOs = guests.Select(g => new GuestDTO
            {
                GuestId = g.GuestId,
                Name = g.Name,
                Email = g.Email,
                RSVP_Status = g.RSVP_Status,
                Side = g.Side
            });

            return Ok(guestDTOs);
        }

        /// <summary>
        /// Returns a Guest by ID
        /// </summary>
        /// <param name="id">Guest ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// {GuestDTO}<br/>
        /// 404 Not Found if guest does not exist
        /// </returns>
        /// <example>
        /// GET: api/GuestAPI/GetGuestById?id=5
        /// </example>
        [HttpGet("GetGuestById")]
        public async Task<ActionResult<GuestDTO>> FindGuest(int id)
        {
            var guest = await _guestService.GetGuestById(id);
            if (guest == null) return NotFound();

            var guestDTO = new GuestDTO
            {
                GuestId = guest.GuestId,
                Name = guest.Name,
                Email = guest.Email,
                RSVP_Status = guest.RSVP_Status,
                Side = guest.Side
            };

            return Ok(guestDTO);
        }

        /// <summary>
        /// Updates an existing Guest
        /// </summary>
        /// <param name="id">Guest ID</param>
        /// <param name="updateguest">Updated GuestDTO object</param>
        /// <returns>
        /// 204 No Content<br/>
        /// 400 Bad Request if IDs do not match<br/>
        /// 404 Not Found or 500 Internal Server Error on failure
        /// </returns>
        /// <example>
        /// PUT: api/GuestAPI/UpdateGuest/3<br/>
        /// Body: { "guestId": 3, "name": "Alex", "email": "alex@email.com", "rsvp_Status": "Accepted", "side": "Bride" }
        /// </example>
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
        /// Deletes a Guest by ID
        /// </summary>
        /// <param name="id">Guest ID</param>
        /// <returns>
        /// 200 OK with confirmation message<br/>
        /// 404 Not Found or 500 Internal Server Error on failure
        /// </returns>
        /// <example>
        /// DELETE: api/GuestAPI/DeleteGuest/3
        /// </example>
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
