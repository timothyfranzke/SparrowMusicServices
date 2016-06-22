using System;
using System.Linq;
using System.Web;
using Sparrow.Services.File.Interface;
using Sparrow.Services.Utils.Enum;
using Sparrow.Services.Utils.Interface;

namespace Sparrow.Services.File
{
    public class FileSystemAudioFileManager:FileSystemFileManager, IAudioFileManager
    {
        private readonly FileTypes _fileTypes = new FileTypes();
        public void Create(string folderName, HttpPostedFileBase file, int fileNum, string fileType)
        {
            var path = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}", System.Configuration.ConfigurationManager.AppSettings["ContentFolderPath"], System.Configuration.ConfigurationManager.AppSettings["TrackContainerName"], folderName));
            var type = _fileTypes.Get().FirstOrDefault(i=>i.Key == fileType).Value;
            var fileName = string.Format("{0}{1}", fileNum, type);
            var filePath = string.Format("{0}/{1}", path, fileName);

            try
            {
                const int bufferSize = 65536;
                using (var fs = System.IO.File.Create(filePath))
                {
                    using (var reader = file.InputStream)
                    {
                        var buffer = new byte[bufferSize];
                        int read = -1, pos = 0;
                        do
                        {
                            int len = (file.ContentLength < pos + bufferSize
                                ? file.ContentLength - pos
                                : bufferSize);
                            read = reader.Read(buffer, 0, len);
                            fs.Write(buffer, 0, len);
                            pos += read;
                        } while (read > 0);
                    }
                }
            }
            catch(Exception ex)
            {
               
            }
        }

        public void Create(string folderName, byte[] file, int fileNum, string fileType)
        {
            var path = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}", System.Configuration.ConfigurationManager.AppSettings["ContentFolderPath"], System.Configuration.ConfigurationManager.AppSettings["TrackContainerName"], folderName));
            var type = _fileTypes.Get().FirstOrDefault(i => i.Key == fileType).Value;
            var fileName = string.Format("{0}{1}", fileNum, type);
            var filePath = string.Format("{0}/{1}", path, fileName);
            CreateDirectory(path);

            try
            {
                using (var fs = System.IO.File.Create(filePath))
                {
                    fs.Write(file, 0, file.Length);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Update(string filePath, HttpPostedFileBase file, int fileNum)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string filePath, int fieNum)
        {
            throw new System.NotImplementedException();
        }
    }
}