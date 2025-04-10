using Wedding_Playlist.Models;
namespace Wedding_Playlist.Interfaces
{
    public interface IEventGuestService
    {
        Task<IEnumerable<EventGuest>> GetEventGuests();
        Task<EventGuest> GetEventGuestById(int id);
        Task<List<EventGuest>> GetEventGuestsbyEventId(int id);
        Task<List<EventGuest>> GetEventGuestsbyGuestId(int id);
        Task<ServiceResponse> AddEventGuest(EventGuestDTO eventGuestDTO);
        Task<ServiceResponse> UpdateEventGuest(EventGuestDTO eventGuestDTO);
        Task<ServiceResponse> DeleteEventGuest(int id);
    }
}