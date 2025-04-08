using System.ComponentModel.DataAnnotations;
namespace Wedding_Playlist.Models
{
    public class EventSong
    {
        [Key]
        public int EventSongId { get; set; }
        public int EventId { get; set; }

        public Event Event { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; }
    }
    public class EventSongDTO
    {
        public int EventSongId { get; set; }
        public int EventId { get; set; }
        public int SongId { get; set; }
    }
}
