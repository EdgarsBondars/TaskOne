using System.Net;
using Azure;
using Azure.Data.Tables;

namespace Task1.Storage
{
    /// <summary>
    /// Azure Tables related logic.
    /// </summary>
    public interface ITableLogic
    {
        /// <summary>
        /// Retrieves logs within a specified date range from the specified table.
        /// </summary>
        /// <param name="fromDate">The start date of the date range.</param>
        /// <param name="toDate">The end date of the date range.</param>
        /// <param name="tableName">The name of the table to query.</param>
        /// <returns>A list of TableEntity objects representing the logs within the specified date range.</returns>
        Task<List<TableEntity>> GetLogsInDateRange(DateTime fromDate, DateTime toDate, string tableName);

        /// <summary>
        /// Creates a new entity in the specified table with the provided details.
        /// </summary>
        /// <param name="tableName">The name of the table in which to create the entity.</param>
        /// <param name="statusCode">The HTTP status code associated with the entity.</param>
        /// <param name="blobName">The name of the blob associated with the entity.</param>
        Task<Response> CreateEntity(string tableName, HttpStatusCode statusCode, string blobName);

        /// <summary>
        /// Ensures that the specified table exists.
        /// If the table does not exist, it creates the table.
        /// </summary>
        /// <param name="tableName">The name of the table to ensure exists.</param>
        Task EnsureTableExists(string tableName);
    }
}