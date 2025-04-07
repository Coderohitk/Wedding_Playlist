using Wedding_Playlist.Services;        // For EventService, GuestService
using Wedding_Playlist.Interfaces;      // For IEventService, IGuestService
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Wedding_Playlist.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// ? Register custom services
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    try
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Wedding Playlist API",
            Version = "v1"
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine("Swagger configuration failed:");
        Console.WriteLine(ex.ToString());
        throw; // rethrow to ensure the error still bubbles up
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wedding Playlist API"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
