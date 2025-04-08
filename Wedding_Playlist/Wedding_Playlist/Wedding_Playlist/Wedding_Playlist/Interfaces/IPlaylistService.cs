using Wedding_Playlist.Models;
namespace Wedding_Playlist.Interfaces
{
    public interface IPlaylistService
    {
        Task<IEnumerable<Playlist>> GetAllPlaylists();
        Task<Playlist> GetPlaylist(int id);
        Task<ServiceResponse> CreatePlaylist(PlaylistDTO playlistDTO);
        Task<ServiceResponse> UpdatePlaylist(PlaylistDTO playlistDTO);
        Task<ServiceResponse> DeletePlaylist(int id);

    }
}