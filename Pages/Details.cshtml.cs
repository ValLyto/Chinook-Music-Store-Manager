using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Models;
using Project.Data;
using System.Linq;

namespace Project.Pages
{
    public class DetailsModel : PageModel 
    {
        // These will hold the data we want to show on the screen
        public Album Album { get; set; }
        public Artist Artist { get; set; }
        public List<Track> Tracks { get; set; }

        // This method runs when you open the Details page for a specific album
        public IActionResult OnGet(int? id) 
        {
            // If the ID is missing in the URL, just go back to the home page
            if (id == null)
            {
                return RedirectToPage("/Index");
            }

            using (var db = new ChinookContext()) 
            {
                // 1. Find the album that matches the ID from the URL
                Album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
                
                // Safety check: if the album doesn't exist in the DB, go back to Index
                if (Album == null)
                {
                    return RedirectToPage("/Index");
                }

                // 2. Find the Artist who made this album using the ArtistId link
                Artist = db.Artists.SingleOrDefault(a => a.ArtistId == Album.ArtistId);
                
                // 3. Get all the songs (Tracks) that belong to this specific album
                Tracks = db.Tracks.Where(t => t.AlbumId == id).ToList();
            }

            // Show the page with all the information we gathered
            return Page();
        }
    }
}