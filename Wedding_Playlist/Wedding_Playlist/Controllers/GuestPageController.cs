using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;
using CoreEntityFramework.Services;

namespace Wedding_Playlist.Controllers
{
    public class GuestPageController : Controller
    {
        private readonly IGuestService _guestService;
        private readonly IEventService _eventService;
        private readonly IEventGuestService _eventGuestService;
        private readonly IGuestSongRequestService _guestSongRequestService;
        private readonly ISongService _songService;
        public GuestPageController(IGuestService guestService,IEventService eventService, IEventGuestService eventGuestService, IGuestSongRequestService guestSongRequestService,ISongService songService)
        {
            _guestService = guestService;
            _eventService = eventService;
            _eventGuestService = eventGuestService;
            _guestSongRequestService = guestSongRequestService;
            _songService = songService;

        }

        public async Task<IActionResult> Index()
        {
            var guests = await _guestService.GetGuests();

            var guestDTOs = guests.Select(g => new GuestDTO
            {
                GuestId = g.GuestId,
                Name = g.Name,
                Email = g.Email,
                RSVP_Status = g.RSVP_Status,
                Side = g.Side
            });

            return View(guestDTOs);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var guest = await _guestService.GetGuestById(id);
            if (guest == null)
            {
                return NotFound();
            }
            
            // Get events associated with this guest
            var eventGuests = await _eventGuestService.GetEventGuestsbyGuestId(id);
            
            // Get full event details
            List<Event> eventList = new List<Event>();
            foreach (var item in eventGuests)
            {
                var eventData = await _eventService.GetEventById(item.EventId);
                if (eventData != null)
                {
                    eventList.Add(eventData);
                }
            }
            
            // Get the guest's song requests
            var guestSongs = await _guestSongRequestService.GetGuestSongRequestByGuestId(id);
            
            // Create a list of songs with their status
            List<SongWithStatusDTO> songsWithStatus = new List<SongWithStatusDTO>();
            foreach(var request in guestSongs)
            {
                var songData = await _songService.GetSong(request.SongID);
                
                if (songData != null)
                {
                    songsWithStatus.Add(new SongWithStatusDTO
                    {
                        SongId = songData.SongId,
                        Title = songData.Title,
                        Artist = songData.Artist,
                        Genre = songData.Genre,
                        Description = songData.Description,
                        Status = request.Status,
                        RequestID = request.RequestID
                    });
                }
                else
                {
                    ViewData["SongNotFound"] = $"Song with ID {request.SongID} not found";
                }
            }

            ViewData["Guest"] = guest;
            ViewData["GuestName"] = guest.Name;
            ViewData["Events"] = eventList;
            ViewData["EventCount"] = eventList.Count;
            ViewData["SongList"] = songsWithStatus;
            ViewData["SongCount"] = songsWithStatus.Count;
            
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var eventList = await _eventService.GetEvents();
            ViewData["EventsList"] = eventList.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuestDTO guestDTO, int[] selectedEvents)
        {
            if (!ModelState.IsValid)
            {
                var eventList = await _eventService.GetEvents();
                ViewData["EventsList"] = eventList.ToList();
                return View(guestDTO);
            }

            var response = await _guestService.AddGuest(guestDTO);
            if (response.Status != ServiceResponse.ServiceStatus.Created)
            {
                TempData["Error"] = string.Join("; ", response.Messages);
                var eventList = await _eventService.GetEvents();
                ViewData["EventsList"] = eventList.ToList();
                return View(guestDTO);
            }

            // Get the ID of the newly created guest
            int guestId = response.CreatedId;

            // Add event associations if events were selected
            if (selectedEvents != null && selectedEvents.Length > 0)
            {
                foreach (var eventId in selectedEvents)
                {
                    var eventGuestDTO = new EventGuestDTO
                    {
                        EventId = eventId,
                        GuestId = guestId
                    };
                    await _eventGuestService.AddEventGuest(eventGuestDTO);
                }
            }

            TempData["Success"] = "Guest created successfully with selected events.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var guest = await _guestService.GetGuestById(id);
            if (guest == null)
                return NotFound();

            var guestDTO = new GuestDTO
            {
                GuestId = guest.GuestId,
                Name = guest.Name,
                Email = guest.Email,
                RSVP_Status = guest.RSVP_Status,
                Side = guest.Side
            };

            // Get all events for selection
            var allEvents = await _eventService.GetEvents();
            ViewData["AllEvents"] = allEvents;

            // Get events associated with this guest
            var guestEvents = await _eventGuestService.GetEventGuestsbyGuestId(id);
            ViewData["GuestEventIds"] = guestEvents.Select(ge => ge.EventId).ToList();

            return View(guestDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GuestDTO guestDTO, int[] selectedEvents)
        {
            if (id != guestDTO.GuestId)
                return BadRequest();

            // Update the guest basic information
            var response = await _guestService.UpdateGuest(guestDTO);
            if (response.Status != ServiceResponse.ServiceStatus.Updated)
            {
                TempData["Error"] = string.Join("; ", response.Messages);
                
                // Reload events for the view
                var allEvents = await _eventService.GetEvents();
                ViewData["AllEvents"] = allEvents;
                var guestEvents = await _eventGuestService.GetEventGuestsbyGuestId(id);
                ViewData["GuestEventIds"] = guestEvents.Select(ge => ge.EventId).ToList();
                
                return View(guestDTO);
            }

            // Handle event associations
            try
            {
                // Get current event associations
                var currentEventGuests = await _eventGuestService.GetEventGuestsbyGuestId(id);
                var currentEventIds = currentEventGuests.Select(eg => eg.EventId).ToList();
                
                // Events to add
                var eventsToAdd = selectedEvents?.Where(e => !currentEventIds.Contains(e)).ToList() ?? new List<int>();
                
                // Events to remove
                var eventsToRemove = currentEventGuests.Where(eg => selectedEvents == null || !selectedEvents.Contains(eg.EventId)).ToList();
                
                // Remove old associations
                foreach (var eventGuest in eventsToRemove)
                {
                    await _eventGuestService.DeleteEventGuest(eventGuest.EventGuestId);
                }
                
                // Add new associations
                foreach (var eventId in eventsToAdd)
                {
                    var eventGuestDTO = new EventGuestDTO
                    {
                        EventId = eventId,
                        GuestId = id
                    };
                    await _eventGuestService.AddEventGuest(eventGuestDTO);
                }
                
                TempData["Success"] = "Guest and event associations updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating event associations: {ex.Message}";
                
                // Reload events for the view
                var allEvents = await _eventService.GetEvents();
                ViewData["AllEvents"] = allEvents;
                var guestEvents = await _eventGuestService.GetEventGuestsbyGuestId(id);
                ViewData["GuestEventIds"] = guestEvents.Select(ge => ge.EventId).ToList();
                
                return View(guestDTO);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var guest = await _guestService.GetGuestById(id);
            if (guest == null)
                return NotFound();

            return View(new GuestDTO
            {
                GuestId = guest.GuestId,
                Name = guest.Name,
                Email = guest.Email,
                RSVP_Status = guest.RSVP_Status,
                Side = guest.Side
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _guestService.DeleteGuest(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
                return RedirectToAction(nameof(Index));

            TempData["Error"] = string.Join("; ", response.Messages);
            return RedirectToAction("Delete", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> TestSongRequests()
        {
            var allRequests = await _guestSongRequestService.GetAllGuestSongRequests();
            ViewData["RequestCount"] = allRequests?.Count() ?? 0;
            
            List<dynamic> requestDetails = new List<dynamic>();
            if (allRequests != null)
            {
                foreach (var request in allRequests)
                {
                    var song = await _songService.GetSong(request.SongID);
                    requestDetails.Add(new 
                    {
                        RequestID = request.RequestID,
                        GuestID = request.GuestID,
                        SongID = request.SongID,
                        Status = request.Status,
                        SongTitle = song?.Title ?? "Not Found",
                        SongArtist = song?.Artist ?? "Not Found"
                    });
                }
            }
            
            ViewData["RequestDetails"] = requestDetails;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddTestSongRequest(int id)
        {
            var guest = await _guestService.GetGuestById(id);
            var songs = await _songService.GetAllSongs();
            var events = await _eventService.GetEvents();
            
            ViewData["GuestId"] = id;
            ViewData["GuestName"] = guest?.Name ?? "Unknown Guest";
            ViewData["Songs"] = songs?.ToList() ?? new List<Song>();
            ViewData["Events"] = events?.ToList() ?? new List<Event>();
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTestSongRequest(GuestSongRequestDTO request)
        {
            if (request != null)
            {
                var response = await _guestSongRequestService.CreateGuestSongRequest(request);
                if (response.Status == ServiceResponse.ServiceStatus.Created)
                {
                    return RedirectToAction("Details", new { id = request.GuestID });
                }
            }
            
            // If we get here, something went wrong
            return RedirectToAction("Details", new { id = request.GuestID });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSongRequestStatus(int requestId, int guestId, string status)
        {
            // Get the current request
            var request = await _guestSongRequestService.GetGuestSongRequest(requestId);
            if (request == null)
            {
                TempData["Error"] = "Song request not found";
                return RedirectToAction("Details", new { id = guestId });
            }
            
            // Update the status
            var updateDto = new GuestSongRequestDTO
            {
                RequestID = request.RequestID,
                GuestID = request.GuestID,
                EventID = request.EventID,
                SongID = request.SongID,
                Status = status
            };
            
            var response = await _guestSongRequestService.UpdateGuestSongRequest(updateDto);
            
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                if (status == "Approved")
                {
                    TempData["Success"] = "Song request approved and added to event playlist";
                }
                else if (status == "Rejected")
                {
                    TempData["Success"] = "Song request rejected";
                }
            }
            else
            {
                TempData["Error"] = string.Join("; ", response.Messages);
            }
            
            return RedirectToAction("Details", new { id = guestId });
        }
    }
}
