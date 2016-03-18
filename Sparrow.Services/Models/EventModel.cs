using System;

namespace Sparrow.Services.Models
{
    public class EventModel
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Url { get; set; }
    }

    public class CreateEventModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Url { get; set; }
    }

    public class ModifyEventModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int EventId { get; set; }
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Url { get; set; }
    }
}
