using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace AzureBlobStorageApp.BlobHandling
{
    public class BlobManager
    {
        private CloudBlobContainer cloudBlobContainer;
        public BlobManager(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("");
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CloudStorageConnection"].ConnectionString;
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
                if (cloudBlobContainer.CreateIfNotExists())
                {
                    cloudBlobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        });
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        public string UploadFile(HttpPostedFileBase httpPostedFileBase)
        {
            string AbsoluteUri;
            if (httpPostedFileBase == null || httpPostedFileBase.ContentLength == 0)
            {
                return null;
            }
            try
            {
                string fileName = Path.GetFileName(httpPostedFileBase.FileName);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                cloudBlockBlob.Properties.ContentType = httpPostedFileBase.ContentType;
                cloudBlockBlob.UploadFromStream(httpPostedFileBase.InputStream);
                AbsoluteUri = cloudBlockBlob.Uri.AbsoluteUri;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
            return AbsoluteUri;
        }

        public bool DeleteBlob(string AbsoluteUri)
        {

            try
            {
                Uri uri = new Uri(AbsoluteUri);
                string blobName = Path.GetFileName(uri.LocalPath);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
                cloudBlockBlob.Delete();
                return true;

            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        public List<string> BlobList()
        {
            List<string> _liBlob = new List<string>();
            foreach (IListBlobItem item in cloudBlobContainer.ListBlobs())
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob cloudBlockBlob = (CloudBlockBlob)item;
                    _liBlob.Add(cloudBlockBlob.Uri.AbsoluteUri.ToString());
                }
            }
            return _liBlob;
        }
    }
}