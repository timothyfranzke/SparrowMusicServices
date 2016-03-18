using System;
using System.Collections.Generic;

namespace Sparrow.Services.Models
{
    public class CommonModel
    {
        public List<GenreModel> Genres { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GenreModel
    {
        public int GenreId { get; set; }
        public string Genre { get; set; }
    }

    public class CreateGenreModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
    }
}
