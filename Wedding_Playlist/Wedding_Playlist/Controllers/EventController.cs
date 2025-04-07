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
        /// Gets all wedding events.
        /// </summary>
        /// <returns>List of EventDTO objects.</returns>
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
        /// Gets a specific event by ID.
        /// </summary>
        /// <param name="id">The ID of the event.</param>
        /// <returns>An EventDTO object if found; otherwise, 404 NotFound.</returns>

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
        /// Adds a new event to the database.
        /// </summary>
        /// <param name="eventDTO">The event data to add.</param>
        /// <returns>Status with CreatedId or error message.</returns>
        [HttpPost]
        public async Task<ActionResult<EventDTO>> AddEvent(EventDTO eventDTO)
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
        /// Updates an event in the database.
        /// </summary>
        /// <param name="id">The ID of the event to update.</param>
        /// <param name="eventDTO">The updated event data.</param>
        /// <returns>Status with error message if not found.</returns>
        [HttpPut]
        public async Task<ActionResult<ServiceResponse>> UpdateEvent(int id, EventDTO eventDTO)
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
        /// Deletes an event from the database.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        /// <returns>Status with error message if not found.</returns>
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

