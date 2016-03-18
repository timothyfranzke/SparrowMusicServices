using System.Collections.Generic;

namespace Sparrow.Services.Models
{
    public class SearchModel
    {
        public IEnumerable<ArtistModel> Artists { get; set; }
        public IEnumerable<AlbumModel> Albums { get; set; }
        public IEnumerable<TrackModel> Tracks { get; set; }
    }
}
