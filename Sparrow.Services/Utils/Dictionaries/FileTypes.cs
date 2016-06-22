using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparrow.Services.Utils.Enum
{
    public class FileTypes
    {
        private readonly Dictionary<string, string> _fileTypes = new Dictionary<string, string>();

        public FileTypes()
        {
            _fileTypes.Add("audio/mp3", ".mp3");
        }
        public Dictionary<string, string> Get()
        {
            return _fileTypes;
        } 

    }
}