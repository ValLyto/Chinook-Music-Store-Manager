using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Data;
using Project.Models;

namespace Project.Pages
{
    public class InsertModel : PageModel
    {
        // For the dropdown list of artists
        public SelectList Artists { get; set; }

        public void OnGet()
        {
            using (var db = new ChinookContext())
            {
                // Get all artists so the user can pick one from the menu
                var artistsList = db.Artists.OrderBy(a => a.Name).ToList();
                Artists = new SelectList(artistsList, "ArtistId", "Name");
            }
        }

        public IActionResult OnPost()
        {
            // Pick up the data sent from the HTML form
            var title = Request.Form["Title"];
            var artistIdStr = Request.Form["ArtistId"];
            var trackNames = Request.Form["TrackNames"];

            // Make sure the user didn't leave important fields empty
            if (string.IsNullOrEmpty(title) || !int.TryParse(artistIdStr, out int artistId))
            {
                ModelState.AddModelError("", "Please provide a valid Title and Artist.");
                OnGet(); // Reload artist list for the dropdown
                return Page();
            }

            try
            {
                using (var db = new ChinookContext())
                {
                    // 1. Create and save the new Album first
                    var album = new Album 
                    { 
                        Title = title.ToString(), 
                        ArtistId = artistId 
                    };
                    db.Albums.Add(album);
                    db.SaveChanges(); // Save now to generate the new AlbumId

                    // 2. Loop through the song names and add them to the Tracks table
                    foreach (var trackName in trackNames)
                    {
                        if (!string.IsNullOrWhiteSpace(trackName))
                        {
                            db.Tracks.Add(new Track 
                            { 
                                Name = trackName.ToString(),
                                AlbumId = album.AlbumId // Link the song to our new album
                            });
                        }
                    }
                    db.SaveChanges(); // Save all the tracks
                }

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                // If there's a database error, show it to the user
                TempData["Error"] = "Database error: " + ex.Message;
                OnGet();
                return Page();
            }
        }
    }
}