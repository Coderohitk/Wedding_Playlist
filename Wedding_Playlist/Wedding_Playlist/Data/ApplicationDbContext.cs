using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Models;

namespace Wedding_Playlist.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<EventGuest> EventGuests { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<EventSong> EventSongs { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistSong> PlaylistSongs { get; set; }
    public DbSet<GuestSongRequest> GuestSongRequests { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
