using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    // Representation of the 'artists' table
    public class Artist 
    {
        [Key] // The unique ID for each artist
        public int ArtistId { get; set; }
        public string Name { get; set; } = "";
    }

    // Representation of the 'albums' table
    public class Album 
    {
        [Key]
        public int AlbumId { get; set; }
        public string Title { get; set; } = "";
        public int ArtistId { get; set; } // Link to the Artist
    }

    // Representation of the 'tracks' table
    public class Track 
    {
        [Key]
        public int TrackId { get; set; }
        public string Name { get; set; } = "";
        public int AlbumId { get; set; }
        
        // These default values are critical to prevent "NOT NULL" errors in the database
        public int MediaTypeId { get; set; } = 1; 
        public int? GenreId { get; set; } = null;
        public string? Composer { get; set; }
        public int Milliseconds { get; set; } = 180000;
        public int? Bytes { get; set; } = 4000000;
        public decimal UnitPrice { get; set; } = 0.99m;
    }
}