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

        // GET: api/GuestSongRequest
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

        // GET: api/GuestSongRequest/5
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

        // GET: api/GuestSongRequest/details/5
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

            // Return messages that include information about the EventSong addition
            return Ok(new
            {
                Status = response.Status,
                Messages = response.Messages,
                GuestSongRequest = guestSongRequestDTO
            });
        }
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