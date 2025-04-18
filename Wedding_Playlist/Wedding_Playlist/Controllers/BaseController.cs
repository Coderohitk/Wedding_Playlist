using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wedding_Playlist.Interfaces;

namespace Wedding_Playlist.Controllers
{
    public class BaseController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public BaseController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                // Set ShowDashboard to false by default - controllers will override if needed
                ViewBag.ShowDashboard = false;
                
                var dashboardData = await _dashboardService.GetDashboardData();
                
                // Populate ViewBag with dashboard data
                ViewBag.BrideSideGuests = dashboardData.BrideSideGuests;
                ViewBag.GroomSideGuests = dashboardData.GroomSideGuests;
                ViewBag.BothSideGuests = dashboardData.BothSideGuests;
                ViewBag.TotalGuests = dashboardData.TotalGuests;
                
                ViewBag.AcceptedRSVPs = dashboardData.AcceptedRSVPs;
                ViewBag.DeclinedRSVPs = dashboardData.DeclinedRSVPs;
                ViewBag.PendingRSVPs = dashboardData.PendingRSVPs;
                ViewBag.RSVPPercentage = Math.Round(dashboardData.RSVPPercentage);
                
                ViewBag.TotalSongs = dashboardData.TotalSongs;
                ViewBag.SongRequests = dashboardData.SongRequests;
                ViewBag.ApprovedSongs = dashboardData.ApprovedSongs;
                ViewBag.PendingSongs = dashboardData.PendingSongs;
                ViewBag.RejectedSongs = dashboardData.RejectedSongs;
                
                ViewBag.UpcomingEvents = dashboardData.UpcomingEvents;
                ViewBag.AllEvents = dashboardData.AllEvents;
            }
            catch (Exception ex)
            {
                // Log error but continue with execution
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
} 