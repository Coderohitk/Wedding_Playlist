using Wedding_Playlist.Models;

namespace Wedding_Playlist.Interfaces
{
    public interface IGuestSongRequestService
    {
        Task<IEnumerable<GuestSongRequest>> GetAllGuestSongRequests();
        Task<GuestSongRequest> GetGuestSongRequest(int id);
        Task<ServiceResponse> CreateGuestSongRequest(GuestSongRequestDTO guestSongRequestDTO);
        Task<ServiceResponse> UpdateGuestSongRequest(GuestSongRequestDTO guestSongRequestDTO);
        Task<ServiceResponse> DeleteGuestSongRequest(int id);
    }
}