using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sparrow.Services.Utils
{
    public static class File
    {
        private const string AccessKey = "AKIAJDNE4SQR5IKQS7ZQ";
        private const string SecretKey = "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";
        public static bool CreateCloudFile()
        {
            return true;
        }
        public static bool CreateFile(string filePath, HttpPostedFileBase file, int fileNum)
        {
            bool success = false;
            try
            {
                const int bufferSize = 65536;
                using (
                    FileStream fs =
                        System.IO.File.Create(HttpContext.Current.Server.MapPath(filePath + fileNum + ".mp3")))
                {
                    using (Stream reader = file.InputStream)
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

                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public static bool CreateImgFile(string filePath, HttpPostedFileBase file, int fileNum)
        {
            var success = false;
            try
            {
                const int bufferSize = 65536;
                using (
                    FileStream fs =
                        System.IO.File.Create(HttpContext.Current.Server.MapPath(filePath + fileNum + ".jpg")))
                {
                    using (Stream reader = file.InputStream)
                    {
                        var buffer = new byte[bufferSize];
                        int read = -1, pos = 0;
                        do
                        {
                            var len = (file.ContentLength < pos + bufferSize
                                ? file.ContentLength - pos
                                : bufferSize);
                            read = reader.Read(buffer, 0, len);
                            fs.Write(buffer, 0, len);
                            pos += read;
                        } while (read > 0);
                    }
                }

                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public static bool CreateImgFile(string filePath, string base64File, int fileNum)
        {
            var success = false;
            try
            {
                var bytes = Convert.FromBase64String(base64File);
                using (
                    FileStream fs =
                        System.IO.File.Create(HttpContext.Current.Server.MapPath(filePath + fileNum + ".jpg")))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public static bool CreateDirectory(string dirPath)
        {
            bool success = false;
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dirPath));
                    success = true;
                }

            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public static bool CreateCloudDirectory(string dirPath)
        {
            bool success = false;
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dirPath));
                    success = true;
                }

            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Basic operations to work with block blobs
        /// </summary>
        /// <returns>Task<returns>
        public static async Task BasicStorageBlockBlobOperationsAsync(string file, string path, int fileNum, string fileType, string containerName)
        {
            var connection =
                System.Configuration.ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(connection);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            try
            {
                container.CreateIfNotExists();
            }
            catch (StorageException e)
            {
                throw;
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(path + fileNum + fileType);
               

            using (var stream = new MemoryStream(Convert.FromBase64String(file), writable: false))
            {
                blockBlob.UploadFromStream(stream);
                
            }
        }

        /// <summary>
        /// Basic operations to work with block blobs
        /// </summary>
        /// <returns>Task<returns>
        public static async Task BasicStorageBlockBlobOperationsAsync(byte[] file, string path, int fileNum, string fileType, string containerName)
        {
            var connection =
                System.Configuration.ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(connection);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            try
            {
                //await container.CreateIfNotExistsAsync();
            }
            catch (StorageException)
            {
                throw;
            }

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(path + fileNum + fileType);

            using (var stream = new MemoryStream(file, writable:false))
            {
                blockBlob.UploadFromStream(stream);
            }
        }


        public static byte[] GetAudioFile(int? artistId, int? albumId, int? trackId)
        {
            var album = "";
            var track = trackId.ToString() + ".mp3";
            if (albumId == null)
            {
                album = "singles";
            }
            else
            {
                album = albumId.ToString();
            }
            var dir = String.Format("/artists/{0}/albums/{1}/tracks/{2}/{3}", artistId.ToString(), album,
                trackId.ToString(), track);

            var fileLocation = HttpContext.Current.Server.MapPath(dir);
            var bytes = new byte[0];

            using (var fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
            {
                var br = new BinaryReader(fs);
                long numBytes = new FileInfo(fileLocation).Length;
                bytes = br.ReadBytes((int) numBytes);
            }

            return bytes;
        }

        public static byte[] GetImgFile(int? artistId, int? albumId, int? trackId)
        {
            var album = "";
            var track = trackId.ToString() + ".jpg";
            var albumImg = albumId + ".jpg";
            var artistImg = artistId + ".jpg";

            if (albumId == -1)
            {
                album = "singles";
            }
            else
            {
                album = albumId.ToString();
            }
            var trackDir = String.Format("/artists/{0}/albums/{1}/tracks/{2}/img/{3}", artistId.ToString(), album,
                trackId.ToString(), track);
            var albumDir = String.Format("/artists/{0}/albums/{1}/img/{2}", artistId, album, albumImg);
            var artistDir = String.Format("/artists/{0}/img/{1}", artistId, artistImg);
            var sparrowDir = "/artists/default.jpg";
            var fileLocation = String.Empty;

            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(trackDir)) && trackId != null)
            {
                fileLocation = HttpContext.Current.Server.MapPath(trackDir);
            }
            else if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(albumDir)) && albumId != null)
            {
                fileLocation = HttpContext.Current.Server.MapPath(albumDir);
            }
            else if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(artistDir)) && artistId != null)
            {
                fileLocation = HttpContext.Current.Server.MapPath(artistDir);
            }
            else
            {
                fileLocation = HttpContext.Current.Server.MapPath(sparrowDir);
            }
            var bytes = new byte[0];

            using (var fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read))
            {
                var br = new BinaryReader(fs);
                long numBytes = new FileInfo(fileLocation).Length;
                bytes = br.ReadBytes((int) numBytes);
            }

            return bytes;
        }

        /// <summary>
        /// Validates the connection string information in app.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string</param>
        /// <returns>CloudStorageAccount object</returns>
        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
    }
}
