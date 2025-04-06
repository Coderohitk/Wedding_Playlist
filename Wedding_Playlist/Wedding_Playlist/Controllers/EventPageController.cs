using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;
namespace Wedding_Playlist.Controllers
{
    public class EventPageController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IGuestService _guestService;
        public EventPageController(IEventService eventService, IGuestService guestService)
        {
            _eventService = eventService;
            _guestService = guestService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("ListEvent");
        }
        public async Task<IActionResult> ListEvent()
        {
            var events = await _eventService.GetEvents();
            var eventDTO = events.Select(x => new EventDTO()
            {
                EventId = x.EventId,
                Name = x.Name,
                Date = x.Date,
                Location = x.Location,


            }
            );
            return View(eventDTO);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventDTO eventDTO)
        {
            var response = await _eventService.AddEvent(eventDTO);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("ListEvent", "EventPage");
            }
            else
            {
                return RedirectToAction("Create", "EventPage");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventDTO = await _eventService.GetEventById(id);
            if (eventDTO == null)
            {
                return NotFound();
            }
            var model = new EventDTO()
            {
                EventId = eventDTO.EventId,
                Name = eventDTO.Name,
                Date = eventDTO.Date,
                Location = eventDTO.Location
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(int id, EventDTO eventDTO)
        {
            var response = await _eventService.UpdateEvent(eventDTO);
            var eventDTO1 = new EventDTO()
            {
                EventId = eventDTO.EventId,
                Name = eventDTO.Name,
                Date = eventDTO.Date,
                Location = eventDTO.Location
            };
            return View(eventDTO1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var response = await _eventService.DeleteEvent(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("ListEvent", "EventPage");
            }
            else
            {
                return RedirectToAction("Delete", "EventPage");
            }

        }
    }
}


