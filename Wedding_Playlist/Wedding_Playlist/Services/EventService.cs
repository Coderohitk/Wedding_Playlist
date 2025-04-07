using Wedding_Playlist.Interfaces;
using Wedding_Playlist.Models;
using Wedding_Playlist.Data;
using Microsoft.EntityFrameworkCore;
namespace Wedding_Playlist.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Event>> GetEvents()
        {
            List<Event> events = await _context.Events.ToListAsync();
            List<Event> eventlist = new List<Event>();
            foreach (var event1 in events)
            {
                eventlist.Add(new Event()
                {
                    EventId = event1.EventId,
                    Name = event1.Name,
                    Date = event1.Date,
                    Location = event1.Location
                }
                );
            }
            return eventlist;
        }
        public async Task<EventDTO> GetEventById(int id)
        {
            var event1 = await _context.Events.FindAsync(id);
            if (event1 == null)
            {
                return null;
            }
            EventDTO events = new EventDTO()
            {
                EventId = event1.EventId,
                Name = event1.Name,
                Date = event1.Date,
                Location = event1.Location
            };
            return events;
        }
        public async Task<ServiceResponse> AddEvent(EventDTO eventDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Event event1 = new Event()
            {
                EventId = eventDTO.EventId,
                Name = eventDTO.Name,
                Date = eventDTO.Date,
                Location = eventDTO.Location
            };
            try
            {
                _context.Events.Add(event1);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(e.Message);
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = event1.EventId;
            return serviceResponse;
        }
        public async Task<ServiceResponse> UpdateEvent(EventDTO eventDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Event addEvent = new Event()
            {
                EventId = eventDTO.EventId,
                Name = eventDTO.Name,
                Date = eventDTO.Date,
                Location = eventDTO.Location
            };
            _context.Entry(addEvent).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Event Already Exists");
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeleteEvent(int id)
        {
            ServiceResponse response = new ServiceResponse();
            var event1 = await _context.Events.FindAsync(id);
            if (event1 == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Event Not Found");
                return response;

            }
            try
            {
                _context.Events.Remove(event1);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add(e.Message);
                return response;
            }
            response.Status = ServiceResponse.ServiceStatus.Deleted;
            return response;
        }
    }
}
