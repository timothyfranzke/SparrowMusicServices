using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sparrow.Services.Models
{
    public class PlaylistModel
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public List<PlaylistTrack> Tracks { get; set; }
        public List<string> Genres { get; set; }

    }

    public class PlaylistTrack
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public int? AlbumId { get; set; }
        public string AlbumName { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; }
        public List<int> Genres { get; set; }
        public double PopIndex { get; set; }
        public MarketModel Market { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Setting { get; set; }
        public Dictionary<string, string> Settings { get; set; } 
    }

    public class PlaylistCachePageModel : ICloneable
    {
        public List<PlaylistTrack> Tracks { get; set; } 
        public int Page { get; set; }
        public object Clone()
        {
            return JsonConvert.DeserializeObject<PlaylistCachePageModel>(JsonConvert.SerializeObject(this));
        }
    }

    public class PlaylistCacheModel
    {
        public int Total { get; set; }
        public List<PlaylistCachePageModel> Pages { get; set; } 
    }

    public class PlaylistPageModel
    {
        public long PlaylistID { get; set; }
        public int TotalPages { get; set; }
    }
}
