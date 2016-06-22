using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Sparrow.Services.File.Interface;
using Sparrow.Services.Utils.Interface;

namespace Sparrow.Services.File
{
    public class AzureImageFileManager:AzureFileManager, IImageFileManager
    {
        private static string _containerName;

        public AzureImageFileManager()
        {
            _containerName = System.Configuration.ConfigurationManager.AppSettings["ImageContainerName"];
        }

        public AzureImageFileManager(string containerName)
        {
            _containerName = containerName;
        }

        public void Create(string filePath, string base64File, int fileNum)
        {
            var fileType = ".jpg";
            var connection =
                System.Configuration.ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(connection);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(_containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath + fileNum + fileType);
            var bytes = Convert.FromBase64String(base64File);
            using (var fs = System.IO.File.Create(HttpContext.Current.Server.MapPath(filePath)))
            {
                fs.Write(bytes, 0, bytes.Length);
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
    }
}