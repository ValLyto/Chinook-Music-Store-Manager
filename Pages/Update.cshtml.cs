using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models;
using Project.Data;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Pages
{
    // The main class for the update page
    public class UpdateModel : PageModel
    {
        // This is to hold the album data for editing
        public Album Album { get; set; }
        
        // This is to hold the album's tracks
        public List<Track> Tracks { get; set; }
        
        // This is a list of all artists, for the dropdown menu
        public SelectList Artists { get; set; }

        // This runs when the page first loads
        public void OnGet(int id)
        {
            // First, make sure we have a fresh connection to the database
            using (var db = new ChinookContext())
            {
                // Find the existing album in the database by its ID
                Album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
                
                // If we didn't find the album, go back to the main page
                if (Album == null)
                {
                    RedirectToPage("Index");
                    return;
                }

                // Also find all the tracks that belong to this album
                Tracks = db.Tracks.Where(t => t.AlbumId == id).ToList();

                // Now get all artists and order them by name
                var artistsList = db.Artists.OrderBy(a => a.Name).ToList();
                
                // Create the SelectList for the dropdown menu, selecting the current artist
                Artists = new SelectList(artistsList, "ArtistId", "Name", Album.ArtistId);
            }
        }

        // This runs when the user clicks the save button
        public IActionResult OnPost()
        {
            // Get the album ID from the hidden form field
            var idStr = Request.Form["id"];
            var title = Request.Form["Title"].ToString();
            var artistIdStr = Request.Form["ArtistId"];

            // Now, get the fresh connection to the DB
            using (var db = new ChinookContext())
            {
                int id = int.Parse(idStr);
                
                // Find the existing album in the DB again to make sure it's up-to-date
                var albumToUpdate = db.Albums.SingleOrDefault(a => a.AlbumId == id);

                // If the album is gone, we can't update it
                if (albumToUpdate == null)
                {
                    // This could be a race condition, so let's go back and reload
                    return RedirectToPage("Index");
                }

                // First, validate the album title. Must be something and under 160 chars.
                if (string.IsNullOrEmpty(title) || title.Length > 160)
                {
                    // If validation fails, we need to show the error
                    ModelState.AddModelError("Album.Title", "Title is required and must be under 160 characters.");
                }

                // Now let's handle the tracks. Find the existing tracks for the album.
                var existingTracks = db.Tracks.Where(t => t.AlbumId == id).ToList();
                
                // Iterate through the existing tracks to check for updates
                foreach (var track in existingTracks)
                {
                    // Create the form field name for this specific track (e.g., Track_10)
                    string formFieldName = "Track_" + track.TrackId;
                    
                    // Now try to find the track name in the form data
                    string submittedTrackName = Request.Form[formFieldName];

                    // If a name was submitted for this track
                    if (submittedTrackName != null)
                    {
                        // Clean up any extra spaces
                        submittedTrackName = submittedTrackName.Trim();

                        // Basic validation: the track name cannot be empty
                        if (string.IsNullOrEmpty(submittedTrackName))
                        {
                            // If a track name is empty, add an error to the page
                            ModelState.AddModelError("", $"Track name for ID {track.TrackId} cannot be empty.");
                        }
                        else
                        {
                            // If the track name is different, update the track name
                            if (track.Name != submittedTrackName)
                            {
                                track.Name = submittedTrackName;
                            }
                        }
                    }
                }

                // Before we save, let's see if there are any model validation errors
                if (!ModelState.IsValid)
                {
                    // If there are errors, we need to reload the page with errors.
                    // Call OnGet with the album ID to reload the original data.
                    OnGet(id);
                    // Now manually set the album title from the form back onto the Model
                    // so the user doesn't lose their edited title. The artist is still good from OnGet.
                    // Model.Album is not null as OnGet would have found the original.
                    Album.Title = title;
                    return Page();
                }

                // If everything is valid, we can update the album and save all changes.
                albumToUpdate.Title = title;
                albumToUpdate.ArtistId = int.Parse(artistIdStr);
                
                // Now save all changes to the DB in one shot
                db.SaveChanges();
            }

            // Go back to the main list after a successful update
            return RedirectToPage("Index");
        }
    }
}