using Wedding_Playlist.Models;

namespace Wedding_Playlist.Interfaces
{
    public interface IPlaylistSongService
    {
        Task<IEnumerable<PlaylistSong>> GetAllPlaylistSongs();
        Task<PlaylistSong> GetPlaylistSong(int id);
        Task<IEnumerable<PlaylistSong>> GetPlaylistSongsByPlaylistId(int playlistId);
        Task<ServiceResponse> CreatePlaylistSong(PlaylistSongDTO playlistSongDTO);
        Task<ServiceResponse> UpdatePlaylistSong(PlaylistSongDTO playlistSongDTO);
        Task<ServiceResponse> DeletePlaylistSong(int id);
    }
}