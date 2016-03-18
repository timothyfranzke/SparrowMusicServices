using System.Web;

namespace Sparrow.Services.Models
{
    public class ImageModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int ArtistId { get; set; }
        public int TrackingId { get; set; }
        public string Name { get; set; }
        public bool Primary { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase AlbumImage { get; set; }
    }

    public class ImageBase64Model
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
        public int TrackingId { get; set; }
        public string Name { get; set; }
        public bool Primary { get; set; }
        public string Description { get; set; }
        public string Image64 { get; set; }
    }
}
