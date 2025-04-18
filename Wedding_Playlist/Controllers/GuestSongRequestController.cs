using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;

namespace Wedding_Playlist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestSongRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGuestSongRequestService _guestSongRequestService;

        public GuestSongRequestController(ApplicationDbContext context, IGuestSongRequestService guestSongRequestService)
        {
            _context = context;
            _guestSongRequestService = guestSongRequestService;
        }

        /// <summary>
        /// Returns a list of all Guest Song Requests
        /// </summary>
        /// <returns>
        /// 200 OK<br/>
        /// [{GuestSongRequestDTO}, {GuestSongRequestDTO}, ...]
        /// </returns>
        /// <example>
        /// GET: api/GuestSongRequest
        /// </example>
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
        /// Returns a Guest Song Request by ID
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// {GuestSongRequestDTO}<br/>
        /// 404 Not Found if not found
        /// </returns>
        /// <example>
        /// GET: api/GuestSongRequest/5
        /// </example>
        [HttpGet("{id}")]
        public async Task<ActionResult<GuestSongRequestDTO>> GetGuestSongRequest(int id)
        {
            var guestSongRequest = await _context.GuestSongRequests
                .FirstOrDefaultAsync(s => s.RequestID == id);

            if (guestSongRequest == null)
            {
                return NotFound();
            }

            var dto = new GuestSongRequestDTO
            {
                RequestID = guestSongRequest.RequestID,
                EventID = guestSongRequest.EventID,
                GuestID = guestSongRequest.GuestID,
                SongID = guestSongRequest.SongID,
                Status = guestSongRequest.Status
            };

            return Ok(dto);
        }

        /// <summary>
        /// Returns detailed information for a Guest Song Request by ID using the service layer
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// ServiceResponse with additional metadata<br/>
        /// 404 Not Found if request doesn't exist
        /// </returns>
        /// <example>
        /// GET: api/GuestSongRequest/details/5
        /// </example>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetGuestSongRequestById(int id)
        {
            var response = await _guestSongRequestService.GetGuestSongRequestById(id);

            if (!response.Success)
            {
                return NotFound(new { message = response.Messages.FirstOrDefault() });
            }

            return Ok(response);
        }

        /// <summary>
        /// Creates a new Guest Song Request
        /// </summary>
        /// <param name="guestSongRequestDTO">GuestSongRequestDTO object</param>
        /// <returns>
        /// 201 Created<br/>
        /// URI to the newly created GuestSongRequest
        /// </returns>
        /// <example>
        /// POST: api/GuestSongRequest<br/>
        /// Body: { "eventID": 1, "guestID": 2, "songID": 5, "status": "Pending" }
        /// </example>
        [HttpPost]
        public async Task<ActionResult<GuestSongRequestDTO>> CreateGuestSongRequest([FromBody] GuestSongRequestDTO guestSongRequestDTO)
        {
            if (guestSongRequestDTO == null)
            {
                return BadRequest();
            }

            var newRequest = new GuestSongRequest
            {
                EventID = guestSongRequestDTO.EventID,
                GuestID = guestSongRequestDTO.GuestID,
                SongID = guestSongRequestDTO.SongID,
                Status = guestSongRequestDTO.Status
            };

            _context.GuestSongRequests.Add(newRequest);
            await _context.SaveChangesAsync();

            guestSongRequestDTO.RequestID = newRequest.RequestID;

            return CreatedAtAction(nameof(GetGuestSongRequest), new { id = newRequest.RequestID }, guestSongRequestDTO);
        }

        /// <summary>
        /// Updates an existing Guest Song Request
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <param name="guestSongRequestDTO">Updated GuestSongRequestDTO</param>
        /// <returns>
        /// 200 OK with status and updated DTO<br/>
        /// 400 Bad Request if ID mismatch<br/>
        /// 404 Not Found if request doesn't exist
        /// </returns>
        /// <example>
        /// PUT: api/GuestSongRequest/7<br/>
        /// Body: { "requestID": 7, "eventID": 1, "guestID": 2, "songID": 5, "status": "Approved" }
        /// </example>
        [HttpPut("{id}")]
        public async Task<ActionResult<GuestSongRequestDTO>> UpdateGuestSongRequest([FromRoute] int id, [FromBody] GuestSongRequestDTO guestSongRequestDTO)
        {
            if (id != guestSongRequestDTO.RequestID)
            {
                return BadRequest();
            }

            var response = await _guestSongRequestService.UpdateGuestSongRequest(guestSongRequestDTO);

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return NotFound();
            }

            return Ok(new
            {
                Status = response.Status,
                Messages = response.Messages,
                GuestSongRequest = guestSongRequestDTO
            });
        }

        /// <summary>
        /// Deletes a Guest Song Request by ID
        /// </summary>
        /// <param name="id">Request ID</param>
        /// <returns>
        /// 200 OK with deleted GuestSongRequest object<br/>
        /// 404 Not Found if not found
        /// </returns>
        /// <example>
        /// DELETE: api/GuestSongRequest/4
        /// </example>
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
