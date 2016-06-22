using System;
using System.Collections.Generic;

namespace Sparrow.Services.Models
{
    public class AlbumModel
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<TrackModel> Tracks { get; set; }
        public bool HasImage { get; set; }
        public int? ImgId { get; set; }
    }

    public class CreateAlbumModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int ArtistId { get; set; }
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }

    }

    public class ModifyAlbumModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }

    }
}
