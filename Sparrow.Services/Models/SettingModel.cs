using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparrow.Services.Models
{
    public class SettingModel
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public int ArtistId { get; set; }
        public string Setting { get; set; }
    }
}