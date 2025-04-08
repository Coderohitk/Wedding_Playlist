using System.ComponentModel.DataAnnotations;

namespace Wedding_Playlist.Models
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }

        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public ICollection<EventSong> EventSongs { get; set; } = new List<EventSong>();
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
        public ICollection<GuestSongRequest> GuestSongRequests { get; set; } = new List<GuestSongRequest>();
    }
    public class SongDTO
    {
        public int SongId { get; set; }

        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
    }
    public class SongWithStatusDTO
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int RequestID { get; set; }
    }

    public class SongCreateViewModel
    {
        public SongDTO Song { get; set; }
        public List<EventSelection> EventSelections { get; set; } = new List<EventSelection>();
        public List<PlaylistSelection> PlaylistSelections { get; set; } = new List<PlaylistSelection>();
    }

    public class EventSelection
    {
        public int EventId { get; set; }
        public bool IsSelected { get; set; }
    }

    public class PlaylistSelection
    {
        public int PlaylistID { get; set; }
        public bool IsSelected { get; set; }
    }
}
