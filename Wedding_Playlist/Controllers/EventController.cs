using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;

namespace Wedding_Playlist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Returns a list of Events
        /// </summary>
        /// <returns>
        /// 200 OK<br/>
        /// [{EventDTO},{EventDTO},..]
        /// </returns>
        /// <example>
        /// GET: api/Event -> [{EventDTO},{EventDTO},..]
        /// </example>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents()
        {
            var events = await _eventService.GetEvents();
            var eventDTOs = events.Select(e => new EventDTO
            {
                EventId = e.EventId,
                Name = e.Name,
                Date = e.Date,
                Location = e.Location
            });

            return Ok(eventDTOs);
        }

        /// <summary>
        /// Returns an Event by its ID
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// {EventDTO}<br/>
        /// 404 Not Found if event does not exist
        /// </returns>
        /// <example>
        /// GET: api/Event/5 -> {EventDTO}
        /// </example>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEventsById(int id)
        {
            var e = await _eventService.GetEventById(id);
            if (e == null) return NotFound();

            var eventDTO = new EventDTO
            {
                EventId = e.EventId,
                Name = e.Name,
                Date = e.Date,
                Location = e.Location
            };

            return Ok(eventDTO);
        }

        /// <summary>
        /// Adds a new Event
        /// </summary>
        /// <param name="eventDTO">EventDTO object</param>
        /// <returns>
        /// 200 OK<br/>
        /// ID of the created event<br/>
        /// 400 Bad Request or 404 Not Found on error
        /// </returns>
        /// <example>
        /// POST: api/Event<br/>
        /// Body: { "name": "Wedding", "date": "2025-04-18", "location": "Toronto" }
        /// </example>
        [HttpPost]
        public async Task<ActionResult<EventDTO>> AddEvent([FromBody] EventDTO eventDTO)
        {
            ServiceResponse response = await _eventService.AddEvent(eventDTO);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(response.Messages);
            }
            return Ok(response.CreatedId);
        }

        /// <summary>
        /// Updates an existing Event
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <param name="eventDTO">Updated EventDTO object</param>
        /// <returns>
        /// 200 OK<br/>
        /// Updated status message<br/>
        /// 400 Bad Request or 404 Not Found on error
        /// </returns>
        /// <example>
        /// PUT: api/Event?id=5<br/>
        /// Body: { "eventId": 5, "name": "Updated Wedding", "date": "2025-05-01", "location": "Vancouver" }
        /// </example>
        [HttpPut]
        public async Task<ActionResult<ServiceResponse>> UpdateEvent(int id, [FromBody] EventDTO eventDTO)
        {
            if (id != eventDTO.EventId)
            {
                return BadRequest();
            }

            ServiceResponse response = await _eventService.UpdateEvent(eventDTO);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(response.Messages);
            }
            return Ok(response);
        }

        /// <summary>
        /// Deletes an Event by its ID
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// Deleted status message<br/>
        /// 400 Bad Request or 404 Not Found on error
        /// </returns>
        /// <example>
        /// DELETE: api/Event/5
        /// </example>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse>> DeleteEvent(int id)
        {
            ServiceResponse response = await _eventService.DeleteEvent(id);
            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(response.Messages);
            }
            return Ok(response);
        }
    }
}
