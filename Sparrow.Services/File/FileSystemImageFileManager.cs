using System;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;
using System.Web;
using Sparrow.Services.File.Interface;
using Sparrow.Services.Utils.Interface;

namespace Sparrow.Services.File
{
    public class FileSystemImageFileManager : FileSystemFileManager, IImageFileManager
    {
        public void Create(string folderName, string base64File, int fileNum)
        {

            //var path = string.Format("{0}/{1}/{2}", System.Configuration.ConfigurationManager.AppSettings["ContentFolderPath"], System.Configuration.ConfigurationManager.AppSettings["ImageContainerName"], folderName);
            var path = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}", System.Configuration.ConfigurationManager.AppSettings["ContentFolderPath"], System.Configuration.ConfigurationManager.AppSettings["ImageContainerName"], folderName));
            var fileName = string.Format("{0}.jpg", fileNum);
            var filePath = string.Format("{0}/{1}", path, fileName);
            try
            {
                var bytes = Convert.FromBase64String(base64File);
                CreateDirectory(path);

                using (var fs = System.IO.File.Create(filePath))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch
            {
                
            }
        }

        public void Update(string filePath, string base64File, int fileNum)
        {
            throw new NotImplementedException();
        }

        public void Delete(string filePath, int fileNum)
        {
            throw new NotImplementedException();
        }

        public void Delete(int fileNum)
        {
            throw new NotImplementedException();
        }
    }
}