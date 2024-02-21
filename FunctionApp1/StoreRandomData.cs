using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Task1.Storage;

[assembly: FunctionsStartup(typeof(StoreRandomData.Startup))]
namespace FunctionApp1
{
    /// <summary>
    /// Azure Function to store random data retrieved from a public API.
    /// </summary>
    public class StoreRandomData
    {
        private HttpClient _httpClient;
        private readonly IBlobLogic _blobLogic;
        private readonly ITableLogic _tableLogic;
        private static string RandomDataApiUrl = "https://api.publicapis.org/random?auth=null";

        /// <summary>
        /// Azure Function to store random data retrieved from a public API.
        /// </summary>
        /// <param name="blobLogic">The IBlobLogic implementation for interacting with Azure Blob Storage.</param>
        /// <param name="tableLogic">The ITableLogic implementation for interacting with Azure Table Storage.</param>
        public StoreRandomData(IBlobLogic blobLogic, ITableLogic tableLogic)
        {
            _httpClient = new HttpClient();
            _blobLogic = blobLogic;
            _tableLogic = tableLogic;

            _tableLogic.EnsureTableExists(Tables.LogTable);
        }

        /// <summary>
        /// Azure Function to store random data retrieved from a public API.
        /// </summary>
        /// <param name="myTimer">Timer information.</param>
        /// <param name="log">Logger instance.</param>
        [FunctionName("StoreRandomData")]
        public async Task Run([TimerTrigger("0 * * * * * ")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function started at: {DateTime.Now}");

            var result = await _httpClient.GetAsync(RandomDataUrl);

            string blobName = $"payload_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            Task uploadBlobTask = _blobLogic.UploadBlob(await result.Content.ReadAsStringAsync(), blobName);
            Task CreateTableEntityTask = _tableLogic.CreateEntity(Tables.LogTable, result.StatusCode, blobName);

            await Task.WhenAll(uploadBlobTask, CreateTableEntityTask);
        }
    }
}
