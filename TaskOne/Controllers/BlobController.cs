using Microsoft.AspNetCore.Mvc;
using Task1.Storage;

namespace Task1.Controllers
{
    /// <summary>
    /// Controller for handling operations related to blobs.
    /// </summary>
    [ApiController]
    [Route("Blob")]
    public class BlobController : ControllerBase
    {
        private readonly IBlobLogic _blobLogic;

        /// <summary>
        /// Controller for handling operations related to blobs.
        /// </summary>
        public BlobController(IBlobLogic blobLogic)
        {
            _blobLogic = blobLogic;
        }

        /// <summary>
        /// Retrieves data from a blob.
        /// </summary>
        /// <param name="blobName">The name of the blob to retrieve data from.</param>
        /// <returns>The data stored in the specified blob.</returns>
        [HttpGet(Name = "GetDataFromBlob")]
        public async Task<string> GetDataFromPayloadBlob(string blobName)
        {
            return await _blobLogic.GetDataFromBlob(blobName, Blobs.PayloadBlob);
        }
    }
}
