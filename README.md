# 🎵 Chinook Music Store Manager

**Author:** Valeriia Lytovka (Banner ID: B01682412)  
**Grade:** 62/100 (Pass) 

## 📖 About the Project
The Chinook Music Store Manager is a web-based inventory management system designed for record store staff. It connects to the Chinook SQLite database, allowing users to effortlessly browse, search, and manage a vast catalog of artists, albums, and tracks through a clean, modern web interface.

## ✨ Features
- **Full CRUD Operations:** Seamlessly Create, Read, Update, and Delete records for Albums and their associated Tracks.
- **Smart Search:** Quick search functionality to find music by either Album Title or Artist Name.
- **Responsive UI:** A clean, intuitive, and mobile-friendly interface built with Bootstrap 5.
- **User Documentation:** Includes a comprehensive, step-by-step User Manual with screenshots.

## 🛠️ Built With
- **Backend:** C#, ASP.NET Core (Razor Pages)
- **Database:** SQLite, Entity Framework Core
- **Frontend:** HTML5, CSS3, Bootstrap 5

## 🚧 Known Issues (Room for Improvement)
*As noted in the assignment feedback, I am aware of the following limitations in this version:*
- **Foreign Key Constraints (Error 19):** Deleting an album might fail if its associated tracks are linked to global playlists. A cascading delete across all related tables is required for a complete fix.
- **Limited Update Functionality:** Currently, the system allows editing existing track names during an album update, but does not support adding brand-new tracks to an already existing album.
- **Data Sorting:** The application lacks interactive UI sorting (e.g., clicking column headers), though the data is pre-sorted alphabetically by default.
