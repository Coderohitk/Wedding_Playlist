using Wedding_Playlist.Models;
namespace Wedding_Playlist.Interfaces
{
    public interface IEventGuestService
    {
        Task<IEnumerable<EventGuest>> GetEventGuests();
        Task<EventGuest> GetEventGuestById(int id);
        Task<List<EventGuest>> GetEventGuestByEvent(int eventId);
        Task<List<EventGuest>> GetEventGuestByGuest(int guestId);
        Task<List<EventGuest>> GetEventGuestsByIsRSVPAccepted(bool isRSVPAccepted);
        Task<List<EventGuest>> GetEventGuestsByEventMen(int eventMen);
        Task<List<EventGuest>> GetEventGuestsByEventWomen(int eventWomen);
        Task<List<EventGuest>> GetEventGuestsByEventKids(int eventKids);
        Task<ServiceResponse> AddEventGuest(EventGuestDTO eventGuestDTO);
        Task<ServiceResponse> UpdateEventGuest(EventGuestDTO eventGuestDTO);
        Task<ServiceResponse> DeleteEventGuest(int id);
    }
}