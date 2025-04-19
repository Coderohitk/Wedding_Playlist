
# 🎶 Wedding Playlist - Song & Playlist Module

Welcome to the **Wedding Playlist** project! This web application is built using **ASP.NET Core MVC** and **Entity Framework Core**. It helps users manage songs, playlists, and their associations with events and guest requests—perfect for weddings, parties, or any event with a personalized playlist.

---

## 📌 Features

- 🎵 **Song Management**: Create, view, edit, and delete songs.
- 📂 **Playlist Management**: Link songs to specific playlists.
- 📅 **Event Integration**: Connect songs to events using many-to-many relationships.
- 🧑‍🤝‍🧑 **Guest Requests**: Track songs requested by guests.
- 🔍 **Detailed Views**: See full song info with linked events, playlists, and guest data.
- ✅ **Validation & Error Handling**: Built-in model validation and robust error responses.

---

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core MVC (.NET 8)
- **ORM**: Entity Framework Core (Code-First + LINQ)
- **Database**: SQL Server / SQLite
- **Front-End**: Razor Views, Bootstrap (optional)
- **Languages**: C#, HTML, CSS

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Visual Studio 2022+ or VS Code
- SQL Server or SQLite
- (Optional) Postman for API testing

---

### Setup Instructions

#### 1. Clone the repository

```bash
git clone https://github.com/Coderohitk/Wedding_Playlist.git
```

#### 2. Navigate to the project folder

```bash
cd milestone-manager
```

#### 3. Restore NuGet packages

```bash
dotnet restore
```

#### 4. Apply migrations and update the database

```bash
dotnet ef database update
```

#### 5. Run the application

```bash
dotnet run
```

#### 6. Open your browser

Go to:

```
https://localhost:5001
```

or

```
http://localhost:5000
```

---

## 📁 Project Structure

```plaintext
MilestoneManager/
├── Controllers/
│   └── SongPageController.cs
├── Models/
│   ├── Song.cs
│   ├── Playlist.cs
│   ├── EventSong.cs
│   ├── GuestSongRequest.cs
│   └── PlaylistSong.cs
├── Interfaces/
│   └── ISongService.cs
│   └── IPlaylistService.cs
│   └── IEventSongService.cs
│   └── IGuestSongRequestService.cs
│   └── IGuestService.cs
│   └── IEventService.cs
│   └── IPlaylistSongService.cs
│   └── IEventGuestService.cs
├── Services/
│   └── SongService.cs
│   └── PlaylistService.cs
│   └── EventSongService.cs
│   └── GuestSongRequestService.cs
│   └── GuestService.cs
│   └── EventService.cs
│   └── PlaylistSongService.cs
│   └── EventGuestService.cs
├── Views/
│   └── SongPage/
│       ├── ListSong.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       └── Delete.cshtml
└── EventPage/
│       ├── ListSong.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       └── Delete.cshtml
└── GuestPage/
│       ├── ListSong.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       └── Delete.cshtml
└── PlaylistPage/
│       ├── ListSong.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       └── Delete.cshtml
├── Data/
│   └── ApplicationDbContext.cs
└── wwwroot/
```

---
## 📁Additional Features
- Authorization to Add, Edit and Delete Data
- Filtering on GuestSongRequest based on status of the songs
- Search functionality to search a user in GuestSongRequest
- Functional Dashboard on Home Page
- 
## 👥 Team Members & Responsibilities

### 🎸 Sarrah Gandhi
- Developed **Event** and **Guest** models, controllers, views, and API endpoints.
- Implemented **Guest/Event** many-to-many relationship via `EventGuest`.
- Styled all related **views and partials** using Razor and Bootstrap.
- Worked on **authentication/authorization** using Individual Accounts (Admin roles).
- Contributed to **overall page layouts and CSS styling**.
- Integrated song requests by guests using the `GuestSongRequest` table.

### 🎧 Rohit Kumar
- Developed **Song** and **Playlist** models, services, and controllers.
- Built **SongPageController** for full CRUD functionality.
- Implemented **many-to-many relationships**:  
  - `EventSong` (Events ↔ Songs)  
  - `PlaylistSong` (Playlists ↔ Songs)
- Created views for **List, Create, Edit, Delete** songs.
- Handled **data seeding** and sample entries for testing.
- Wrote **LINQ queries** for related entity display in views.
```

Please follow existing code styles and write unit tests for any new features.

---

or full details.

---

## 📬 Contact

Created by **Sarrah Gandhi**
Created by **Rohit Kumar**

---

> “Where words fail, music speaks.” – _Hans Christian Andersen_
```
