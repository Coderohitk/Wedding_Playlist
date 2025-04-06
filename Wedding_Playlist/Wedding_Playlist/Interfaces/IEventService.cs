using Wedding_Playlist.Models;

namespace Wedding_Playlist.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetEvents();
        Task<EventDTO> GetEventById(int id);
        Task<ServiceResponse> AddEvent(EventDTO eventDTO);
        Task<ServiceResponse> UpdateEvent(EventDTO eventDTO);
        Task<ServiceResponse> DeleteEvent(int id);

    }
}