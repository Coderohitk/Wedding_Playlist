using Microsoft.AspNetCore.Mvc;
using Wedding_Playlist.Interfaces;
using Wedding_Playlist.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Wedding_Playlist.Controllers
{
    public class GuestSongRequestPageController : Controller
    {
        private readonly IGuestSongRequestService _guestSongRequestService;
        private readonly IGuestService _guestService;
        private readonly ISongService _songService;
        private readonly IEventService _eventService;
        private readonly IEventSongService _eventSongService;
        
        public GuestSongRequestPageController(
            IGuestSongRequestService guestSongRequestService,
            IGuestService guestService,
            ISongService songService,
            IEventService eventService,
            IEventSongService eventSongService)
        {
            _guestSongRequestService = guestSongRequestService;
            _guestService = guestService;
            _songService = songService;
            _eventService = eventService;
            _eventSongService = eventSongService;
        }
        
        public async Task<IActionResult> Index()
        {
            var requests = await _guestSongRequestService.GetAllGuestSongRequests();
            var viewModel = new List<GuestSongRequestViewModel>();
            
            foreach (var request in requests)
            {
                var guest = await _guestService.GetGuestById(request.GuestID);
                var song = await _songService.GetSong(request.SongID);
                var evt = await _eventService.GetEventById(request.EventID);
                
                viewModel.Add(new GuestSongRequestViewModel
                {
                    RequestID = request.RequestID,
                    GuestID = request.GuestID,
                    GuestName = guest?.Name ?? "Unknown",
                    SongID = request.SongID,
                    SongTitle = song?.Title ?? "Unknown",
                    SongArtist = song?.Artist ?? "Unknown",
                    EventID = request.EventID,
                    EventName = evt?.Name ?? "Unknown",
                    Status = request.Status
                });
            }
            
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int requestId, string status)
        {
            try
            {
                // Find the request first
                var existingRequest = await _guestSongRequestService.GetGuestSongRequestById(requestId);
                if (existingRequest == null || !existingRequest.Success)
                {
                    TempData["ErrorMessage"] = $"Request with ID {requestId} not found.";
                    return RedirectToAction("Index");
                }

                // Get the request from the repository since it's not in the ServiceResponse
                var requestToUpdate = await _guestSongRequestService.GetGuestSongRequest(requestId);
                if (requestToUpdate == null)
                {
                    TempData["ErrorMessage"] = $"Request with ID {requestId} not found.";
                    return RedirectToAction("Index");
                }

                // Create a DTO with the updated status
                var requestDTO = new GuestSongRequestDTO
                {
                    RequestID = requestToUpdate.RequestID,
                    EventID = requestToUpdate.EventID,
                    GuestID = requestToUpdate.GuestID,
                    SongID = requestToUpdate.SongID,
                    Status = status
                };

                // Update the status
                var result = await _guestSongRequestService.UpdateGuestSongRequest(requestDTO);

                if (result.Status == ServiceResponse.ServiceStatus.Updated)
                {
                    if (status == "Approved")
                    {
                        TempData["SuccessMessage"] = "Song request approved successfully!";
                    }
                    else if (status == "Rejected")
                    {
                        TempData["SuccessMessage"] = "Song request rejected successfully!";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = $"Song request status updated to '{status}' successfully!";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = $"Error updating request: {string.Join(", ", result.Messages)}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error processing request: {ex.Message}";
                if (ex.InnerException != null)
                {
                    TempData["ErrorMessage"] += $" | {ex.InnerException.Message}";
                }
            }

            return RedirectToAction("Index");
        }
    }
    
    public class GuestSongRequestViewModel
    {
        public int RequestID { get; set; }
        public int GuestID { get; set; }
        public string GuestName { get; set; }
        public int SongID { get; set; }
        public string SongTitle { get; set; }
        public string SongArtist { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string Status { get; set; }
    }
} 