
namespace Sparrow.Services.Models
{
    public class FilterModel
    {
        public int FilterId { get; set; }
        public string Name { get; set; }
        public string Filter { get; set; }
    }

    public class CreateFilterModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int FilterId { get; set; }
        public string Name { get; set; }
        public string Filter { get; set; }
    }
}