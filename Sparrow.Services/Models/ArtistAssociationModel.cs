namespace Sparrow.Services.Models
{
    public class CreateArtistAssociation
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int ArtistId { get; set; }
        public int UserId { get; set; }
    }
}
