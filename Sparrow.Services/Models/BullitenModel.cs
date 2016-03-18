namespace Sparrow.Services.Models
{
    public class BullitenModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public string Bulliten { get; set; }
        public int ArtistId { get; set; }
        public string AristName { get; set; }
    }

    public class UserBulliten
    {
        public string Bulliten { get; set; }
        public string AristName { get; set; }
    }
}
