using System;
using System.Collections.Generic;

namespace Wedding_Playlist.Models
{
    public class DashboardData
    {
        // Guest counts
        public int BrideSideGuests { get; set; } = 0;
        public int GroomSideGuests { get; set; } = 0;
        public int BothSideGuests { get; set; } = 0;
        public int TotalGuests { get; set; } = 0;

        // RSVP counts
        public int AcceptedRSVPs { get; set; } = 0;
        public int DeclinedRSVPs { get; set; } = 0;
        public int PendingRSVPs { get; set; } = 0;
        public double RSVPPercentage { get; set; } = 0;

        // Song counts
        public int TotalSongs { get; set; } = 0;
        public int SongRequests { get; set; } = 0;
        public int ApprovedSongs { get; set; } = 0;
        public int PendingSongs { get; set; } = 0;
        public int RejectedSongs { get; set; } = 0;

        // Events
        public List<Event> UpcomingEvents { get; set; } = new List<Event>();
        public List<Event> AllEvents { get; set; } = new List<Event>();
    }
} 