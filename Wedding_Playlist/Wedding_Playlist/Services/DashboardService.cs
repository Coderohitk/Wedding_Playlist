using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreEntityFramework.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardData> GetDashboardData()
        {
            var dashboardData = new DashboardData();

            // Get guest counts
            dashboardData.BrideSideGuests = await _context.Guests.CountAsync(g => g.Side != null && g.Side.ToLower() == "bride");
            dashboardData.GroomSideGuests = await _context.Guests.CountAsync(g => g.Side != null && g.Side.ToLower() == "groom");
            dashboardData.BothSideGuests = await _context.Guests.CountAsync(g => g.Side != null && g.Side.ToLower() == "both");
            dashboardData.TotalGuests = dashboardData.BrideSideGuests + dashboardData.GroomSideGuests + dashboardData.BothSideGuests;

            // Get RSVP counts with case-insensitive comparison
            // Check for both "Accepted" and "Attending" values
            dashboardData.AcceptedRSVPs = await _context.Guests.CountAsync(g => 
                g.RSVP_Status != null && 
                (g.RSVP_Status.ToLower() == "accepted" || 
                 g.RSVP_Status.ToLower() == "attending"));
                 
            dashboardData.DeclinedRSVPs = await _context.Guests.CountAsync(g => 
                g.RSVP_Status != null && 
                (g.RSVP_Status.ToLower() == "declined" || 
                 g.RSVP_Status.ToLower() == "not attending"));
                 
            dashboardData.PendingRSVPs = await _context.Guests.CountAsync(g => 
                g.RSVP_Status == null || 
                g.RSVP_Status.ToLower() == "pending");
            
            if (dashboardData.TotalGuests > 0)
            {
                dashboardData.RSVPPercentage = ((double)(dashboardData.AcceptedRSVPs + dashboardData.DeclinedRSVPs) / dashboardData.TotalGuests) * 100;
            }

            // Get song counts
            dashboardData.TotalSongs = await _context.Songs.CountAsync();
            dashboardData.SongRequests = await _context.GuestSongRequests.CountAsync();
            dashboardData.ApprovedSongs = await _context.GuestSongRequests.CountAsync(s => s.Status != null && s.Status.ToLower() == "approved");
            dashboardData.PendingSongs = await _context.GuestSongRequests.CountAsync(s => s.Status == null || s.Status.ToLower() == "pending");
            dashboardData.RejectedSongs = await _context.GuestSongRequests.CountAsync(s => s.Status != null && s.Status.ToLower() == "rejected");

            // Get upcoming events
            dashboardData.UpcomingEvents = await _context.Events
                .Where(e => e.Date >= DateTime.Today)
                .OrderBy(e => e.Date)
                .Take(3)
                .ToListAsync();

            // Get all events for the dashboard
            dashboardData.AllEvents = await _context.Events
                .OrderBy(e => e.Date)
                .ToListAsync();

            return dashboardData;
        }
    }
} 