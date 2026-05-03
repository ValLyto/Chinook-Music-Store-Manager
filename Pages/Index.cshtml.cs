using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Models;
using Project.Data;
using System.Linq;

namespace Project.Pages
{
    public class IndexModel : PageModel
    {
        // My info for the assignment header
        public string StudentName { get; set; } = "Valeriia Lytovka";
        public string BannerId { get; set; } = "B01682412";

        // A small class to group album info, artist name, and track count together
        public class AlbumView 
        {
            public int AlbumId { get; set; }
            public string Title { get; set; } = "";
            public string ArtistName { get; set; } = "";
            public int TrackCount { get; set; }
        }

        public List<AlbumView> Albums { get; set; } = new();

        // This property captures whatever the user types in the search box
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; } = "";

        public void OnGet()
        {
            using (var db = new ChinookContext())
            {
                // Join Albums and Artists tables to get the full info for the list
                var query = from al in db.Albums
                            join ar in db.Artists on al.ArtistId equals ar.ArtistId
                            select new AlbumView 
                            {
                                AlbumId = al.AlbumId,
                                Title = al.Title,
                                ArtistName = ar.Name,
                                // Count how many tracks belong to this album
                                TrackCount = db.Tracks.Count(t => t.AlbumId == al.AlbumId)
                            };

                // Filter the list if the user searched for something
                if (!string.IsNullOrEmpty(SearchString))
                {
                    query = query.Where(a => a.Title.Contains(SearchString) || a.ArtistName.Contains(SearchString));
                }

                // Sort by title and convert to a list
                Albums = query.OrderBy(a => a.Title).ToList();
            }
        }
    }
}