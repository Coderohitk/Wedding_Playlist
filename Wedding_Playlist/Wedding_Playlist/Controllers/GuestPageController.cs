using Microsoft.AspNetCore.Mvc;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;

namespace Wedding_Playlist.Controllers
{
    public class GuestPageController : Controller
    {
        private readonly IGuestService _guestService;

        public GuestPageController(IGuestService guestService)
        {
            _guestService = guestService;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuestDTO guestDTO)
        {
            if (!ModelState.IsValid)
                return View(guestDTO);

            var response = await _guestService.AddGuest(guestDTO);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
                return RedirectToAction(nameof(Index));

            TempData["Error"] = string.Join("; ", response.Messages);
            return View(guestDTO);
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

            return View(guestDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GuestDTO guestDTO)
        {
            if (id != guestDTO.GuestId)
                return BadRequest();

            var response = await _guestService.UpdateGuest(guestDTO);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
                return RedirectToAction(nameof(Index));

            TempData["Error"] = string.Join("; ", response.Messages);
            return View(guestDTO);
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
    }
}
