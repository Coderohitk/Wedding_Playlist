using System.ComponentModel.DataAnnotations;

namespace Wedding_Playlist.Models
{
    public class EventGuest
    {
        [Key]
        public int EventGuestId { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
    }
    public class EventGuestDTO
    {
        public int EventGuestId { get; set; }
        public int EventId { get; set; }
        public int GuestId { get; set; }
    }
}
