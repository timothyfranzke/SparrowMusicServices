using System;
using System.IO;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Sparrow.Services.File.Interface;
using Sparrow.Services.Utils;
using Sparrow.Services.Utils.Interface;

namespace Sparrow.Services.File
{
    public class AzureAudioFileManager:AzureFileManager, IAudioFileManager
    {
        private static string _containerName;

        public AzureAudioFileManager()
        {
            _containerName = System.Configuration.ConfigurationManager.AppSettings["AzureTrackContainerName"];
        }

        public AzureAudioFileManager(string containerName)
        {
            _containerName = containerName;
        }

        public void Create(string filePath, HttpPostedFileBase file, int fileNum, string fileType)
        {
            var type = Path.GetExtension(file.FileName);
            var connection =
                System.Configuration.ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(connection);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(_containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath + fileNum + fileType);
            blockBlob.UploadFromStream(file.InputStream);
        }

        public void Create(string filePath, byte[] file, int fileNum, string fileType)
        {
            //var fileType = Path.GetExtension(file.FileName);
            var connection =
                System.Configuration.ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(connection);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(_containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath + fileNum + ".mp3");
            blockBlob.UploadFromByteArray(file, 0, file.Length);
        }

        public void Update(string filePath, HttpPostedFileBase file, int fileNum)
        {
            throw new NotImplementedException();
        }

        public void Delete(string filePath, int fileNum)
        {
            throw new NotImplementedException();
        }
    }
}