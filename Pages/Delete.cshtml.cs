using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Models;
using Project.Data;
using System.Linq;

namespace Project.Pages
{
    public class DeleteModel : PageModel
    {
        public Album Album { get; set; }

        // This runs when the page loads
        public void OnGet()
        {
            // Get the ID from the browser address bar
            int id = int.Parse(Request.Query["id"]);
            
            using (var db = new ChinookContext())
            {
                // Find the album so we can show its title on the screen
                Album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
            }
        }

        // This runs when we click the "Confirm Delete" button
        public IActionResult OnPost()
        {
            // Get the ID from the hidden input field in the form
            int id = int.Parse(Request.Form["id"]);
            
            using (var db = new ChinookContext())
            {
                // 1. First, find all tracks belonging to this album
                var tracks = db.Tracks.Where(t => t.AlbumId == id).ToList();
                
                // 2. Delete the tracks first! 
                // We do this because SQLite won't let us delete an album if it has songs.
                if (tracks.Any())
                {
                    db.Tracks.RemoveRange(tracks);
                    db.SaveChanges(); // Save here to clear the tracks from the database
                }

                // 3. Now the album is "empty", so we can find and delete it
                var album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
                if (album != null)
                {
                    db.Albums.Remove(album);
                    db.SaveChanges(); // Save again to delete the album
                }
            }
            
            // Go back to the home page (Index)
            return RedirectToPage("Index"); 
        }
    }
}