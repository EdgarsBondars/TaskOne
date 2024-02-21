using System.Net;
using Azure;
using Azure.Data.Tables;

namespace Task1.Storage
{
    /// <summary>
    /// Azure Tables related logic.
    /// </summary>
    public class TableLogic : ITableLogic
    {
        private readonly TableServiceClient _tableServiceClient;

        /// <summary>
        /// Azure Tables related logic.
        /// </summary>
        /// <param name="tableServiceClient">The TableServiceClient instance used to interact with Azure Table Storage.</param>
        public TableLogic(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        /// <summary>
        /// Retrieves logs within a specified date range from the specified table.
        /// </summary>
        /// <param name="fromDate">The start date of the date range.</param>
        /// <param name="toDate">The end date of the date range.</param>
        /// <param name="tableName">The name of the table to query.</param>
        /// <returns>A list of TableEntity objects representing the logs within the specified date range.</returns>
        public async Task<List<TableEntity>> GetLogsInDateRange(DateTime fromDate, DateTime toDate, string tableName)
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);

            return await tableClient.QueryAsync<TableEntity>(
                entity => entity.Timestamp >= fromDate && entity.Timestamp <= toDate).ToListAsync();
        }

        /// <summary>
        /// Creates a new entity in the specified table with the provided details.
        /// </summary>
        /// <param name="tableName">The name of the table in which to create the entity.</param>
        /// <param name="statusCode">The HTTP status code associated with the entity.</param>
        /// <param name="blobName">The name of the blob associated with the entity.</param>
        public async Task<Response> CreateEntity(string tableName, HttpStatusCode statusCode, string blobName)
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);

            var logEntity = new TableEntity(DateTime.Now.ToString("yyyyMMddHHmmssfff"), statusCode.ToString())
            {
                { "BlobName", blobName }
            };

            return await tableClient.AddEntityAsync(logEntity);
        }

        /// <summary>
        /// Ensures that the specified table exists.
        /// If the table does not exist, it creates the table.
        /// </summary>
        /// <param name="tableName">The name of the table to ensure exists.</param>
        public async Task EnsureTableExists(string tableName)
        {
            TableClient tableClient = _tableServiceClient.GetTableClient(tableName);
            try
            {
                await tableClient.CreateAsync();
            }
            catch (RequestFailedException ex) when (ex.Status == 409)
            {

            }
        }
    }
}
