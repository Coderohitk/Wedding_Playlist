using Microsoft.AspNetCore.Mvc;
using Wedding_Playlist.Interfaces;
using Wedding_Playlist.Models;

using System.Collections.Generic;
using System.Threading.Tasks;
namespace MilestoneManager.Controllers
{
    public class PlaylistPageController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IPlaylistSongService _playlistSongService;
        private readonly ISongService _songService;

        public PlaylistPageController(IPlaylistService playlistService, IPlaylistSongService playlistSongService, ISongService songService)
        {
            _playlistService = playlistService;
            _playlistSongService = playlistSongService;
            _songService = songService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("ListPlaylist");
        }
        public async Task<IActionResult> ListPlaylist()
        {
            var playlists = await _playlistService.GetAllPlaylists();
            var playlistDtos = playlists.Select(playlistItem => new PlaylistDTO
            {
                PlaylistID = playlistItem.PlaylistID,
                Name = playlistItem.Name,
                CreatedBy = playlistItem.CreatedBy
            });
            return View(playlistDtos);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var playlist = await _playlistService.GetPlaylist(id);
            if (playlist == null)
            {
                return NotFound();
            }
            
            // Get all playlist songs along with their song details
            var playlistSongs = await _playlistSongService.GetPlaylistSongsByPlaylistId(id);
            
            // Create view model with playlist info and songs
            var viewModel = new PlaylistDetailsViewModel
            {
                Playlist = new PlaylistDTO
                {
                    PlaylistID = playlist.PlaylistID,
                    Name = playlist.Name,
                    CreatedBy = playlist.CreatedBy
                },
                Songs = playlistSongs.Select(ps => new SongDTO
                {
                    SongId = ps.Song.SongId,
                    Title = ps.Song.Title,
                    Artist = ps.Song.Artist,
                    Genre = ps.Song.Genre,
                    Description = ps.Song.Description
                }).ToList()
            };
            
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<IActionResult> CreatePlaylist(PlaylistDTO playlistDto)
        {
            var response = await _playlistService.CreatePlaylist(playlistDto);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("ListPlaylist", "PlaylistPage");
            }
            else
            {
                return RedirectToAction("Create", "PlaylistPage");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var playlist = await _playlistService.GetPlaylist(id);
            if (playlist == null)
            {
                return NotFound();
            }
            var playlistDto = new PlaylistDTO
            {
                PlaylistID = id,
                Name = playlist.Name,
                CreatedBy = playlist.CreatedBy
            };
            return View(playlistDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPlaylist(PlaylistDTO playlistDto)
        {
            var response = await _playlistService.UpdatePlaylist(playlistDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("ListPlaylist", "PlaylistPage");
            }
            else
            {
                return RedirectToAction("Edit", "PlaylistPage");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _playlistService.GetPlaylist(id);
            if (playlist == null)
            {
                return NotFound(); // Prevents the null reference
            }

            var playlistDto = new PlaylistDTO
            {
                PlaylistID = playlist.PlaylistID,
                Name = playlist.Name,
                CreatedBy = playlist.CreatedBy
            };

            return View(playlistDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlaylist(int id)
        {
            var response = await _playlistService.DeletePlaylist(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("ListPlaylist", "PlaylistPage");
            }
            else
            {
                return RedirectToAction("Delete", "PlaylistPage");
            }
        }


    }
}