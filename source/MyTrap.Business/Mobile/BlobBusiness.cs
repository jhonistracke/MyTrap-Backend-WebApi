using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using MyTrap.Business.Mobile.Contracts;
using MyTrap.Model.Mobile.Request;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace MyTrap.Business.Mobile
{
    public class BlobBusiness : IBlobBusiness
    {
        private CloudBlobContainer GetContainer(string containerName)
        {
            CloudBlobContainer container = null;

            try
            {
                StorageCredentials creds = new StorageCredentials(ConfigurationManager.AppSettings["AzureStorageName"], ConfigurationManager.AppSettings["AzureStorageKey"]);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

                CloudBlobClient client = account.CreateCloudBlobClient();

                container = client.GetContainerReference(containerName);
                container.CreateIfNotExists();
            }
            catch (Exception)
            {
                container = null;
            }

            return container;
        }

        public ImageRequest InsertUserImage(ImageRequest image)
        {
            CloudBlobContainer container = GetContainer(ConfigurationManager.AppSettings["AzureStorageContainerUserImages"]);

            if (container != null && image != null)
            {
                string blobName = GenerateBlobName();

                blobName += GetExtensionFromUri(image.Url);

                var blockBlob = container.GetBlockBlobReference(blobName);

                var webClient = new WebClient();

                byte[] imageBytes = webClient.DownloadData(image.Url);

                MemoryStream stream = new MemoryStream(imageBytes);

                blockBlob.UploadFromStream(stream);

                image.OriginUrl = image.Url;
                image.Url = blockBlob.StorageUri.PrimaryUri.ToString();
            }

            return image;
        }

        private string GenerateBlobName()
        {
            Guid guid = Guid.NewGuid();

            return guid.ToString();
        }

        private string GetExtensionFromUri(string uri)
        {
            return ".jpg";
        }
    }
}