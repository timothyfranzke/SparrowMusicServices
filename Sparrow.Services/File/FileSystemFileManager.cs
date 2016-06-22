using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Sparrow.Services.File
{
    public abstract class FileSystemFileManager
    {
        public void CreateDirectory(string dirPath)
        {
            bool success = false;
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            catch (Exception e)
            {
                
            }
        }
    }
}