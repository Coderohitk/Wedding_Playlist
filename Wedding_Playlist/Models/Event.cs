using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Wedding_Playlist.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime  Date { get; set; }
        public string Location { get; set; }

        public ICollection<EventGuest> EventGuests { get; set; } = new List<EventGuest>();
        public ICollection<EventSong> EventSongs { get; set; } = new List<EventSong>();
    }
    public class EventDTO
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
    }
}

