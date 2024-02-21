using Azure.Storage.Blobs.Models;

namespace Task1.Storage
{
    /// <summary>
    /// Blob related logic - retreival, uploading.
    /// </summary>
    public interface IBlobLogic
    {
        /// <summary>
        /// Retrieves data from a blob in the specified container.
        /// </summary>
        /// <param name="blobName">The name of the blob to retrieve.</param>
        /// <param name="containerName">The name of the container containing the blob.</param>
        /// <returns>The content of the blob as a string.</returns>
        Task<string> GetDataFromBlob(string blobName, string containerName);

        /// <summary>
        /// Uploads a message as a blob with the specified name.
        /// </summary>
        /// <param name="message">The message content to upload.</param>
        /// <param name="blobName">The name of the blob to create.</param>
        /// <returns>The blob content info for the uploaded blob.</returns>
        Task<BlobContentInfo> UploadBlob(string message, string blobName);
    }
}
