
using Wedding_Playlist.Models;

namespace Wedding_Playlist.Interfaces
{
    public interface IGuestService
    {
        Task<IEnumerable<Guest>> GetGuests();
        Task<Guest> GetGuestById(int id);
        Task<ServiceResponse> AddGuest(GuestDTO guestDTO);
        Task<ServiceResponse> UpdateGuest(GuestDTO guestDTO);
        Task<ServiceResponse> DeleteGuest(int id);

    }
}