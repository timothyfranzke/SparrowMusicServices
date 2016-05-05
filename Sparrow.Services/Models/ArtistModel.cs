using System.Collections.Generic;

namespace Sparrow.Services.Models
{
    public class ArtistModel
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string Description { get; set; }
        public List<AlbumModel> Albums { get; set; }
        public List<EventModel> Events { get; set; }
        public string Bulliten { get; set; }
        public bool HasImage { get; set; }
        public List<GenreModel> Genres { get; set; }
        public string Settings { get; set; }

    }

    public class CreateArtistModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string Setting { get; set; }
    }
    public class ModifyArtistModel
    {
        public int ArtistId { get; set; }
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }

    public class FollowedArtistModel
    {
        public List<int> ArtistIds { get; set; }
        public List<int> LikedTrackIds { get; set; } 
        public List<EventModel> Events { get; set; }
        public List<BullitenModel> Bulliten { get; set; }
    }
}
