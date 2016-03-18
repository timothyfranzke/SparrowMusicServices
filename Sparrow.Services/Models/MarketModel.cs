namespace Sparrow.Services.Models
{
    public class MarketModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Zip { get; set; }
    }

    public class MarketSearchModel
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Zip { get; set; }
    }
}