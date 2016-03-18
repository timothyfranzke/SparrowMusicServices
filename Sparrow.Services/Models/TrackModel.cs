using System;
using System.Web;

namespace Sparrow.Services.Models
{
    public class TrackModel
    {
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; }

    }

    public class CreateTrackModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int? AlbumId { get; set; }
        public int ArtistId { get; set; }
        public byte[] Track { get; set; }
        public string TrackName { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }

    }

    public class CreateTrackModelWithFile
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int? AlbumId { get; set; }
        public int ArtistId { get; set; }
        public HttpPostedFileBase Track { get; set; }
        public string TrackName { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }

        
    }

    public class ModifyTrackModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int TrackId { get; set; }
        public int? AlbumId { get; set; }
        public int ArtistId { get; set; }
        public HttpPostedFileBase Track { get; set; }
        public string TrackName { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }

    }
}
