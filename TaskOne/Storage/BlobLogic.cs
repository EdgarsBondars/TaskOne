using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Task1.Storage
{
    /// <summary>
    /// Blob related logic - getting blob data, uploading.
    /// </summary>
    public class BlobLogic : IBlobLogic
    {
        private readonly BlobServiceClient _blobServiceClient;

        /// <summary>
        /// Blob related logic - getting blob data, uploading.
        /// </summary>
        /// <param name="blobServiceClient">Blob service client.</param>
        public BlobLogic(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }


        /// <summary>
        /// Retrieves data from a blob in the specified container.
        /// </summary>
        /// <param name="blobName">The name of the blob to retrieve.</param>
        /// <param name="containerName">The name of the container containing the blob.</param>
        /// <returns>The content of the blob as a string.</returns>
        public async Task<string> GetDataFromBlob(string blobName, string containerName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            using (MemoryStream ms = new MemoryStream())
            {
                await blobClient.DownloadToAsync(ms);
                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        /// <summary>
        /// Uploads a message as a blob with the specified name.
        /// </summary>
        /// <param name="message">The message content to upload.</param>
        /// <param name="blobName">The name of the blob to create.</param>
        /// <returns>The blob content info for the uploaded blob.</returns>
        public async Task<BlobContentInfo> UploadBlob(string message, string blobName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(Blobs.PayloadBlob);
            await EnsureBlobContainerExists(blobContainerClient);

            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            byte[] byteArray = Encoding.UTF8.GetBytes(message);
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return await blobClient.UploadAsync(memoryStream, true);
            }
        }

        /// <summary>
        /// Ensures that the specified blob container exists.
        /// If the container does not exist, it creates the container.
        /// </summary>
        /// <param name="blobContainerClient">The BlobContainerClient instance representing the blob container.</param>
        private async Task EnsureBlobContainerExists(BlobContainerClient blobContainerClient)
        {
            if (!await blobContainerClient.ExistsAsync())
            {
                await blobContainerClient.CreateAsync();
            }
        }
    }
}
