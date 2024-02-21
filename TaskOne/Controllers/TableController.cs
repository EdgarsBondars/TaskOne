using Microsoft.AspNetCore.Mvc;
using Task1.Storage;

namespace Task1.Controllers
{
    /// <summary>
    /// Controller for handling operations related to tables.
    /// </summary>
    [ApiController]
    [Route("Table")]
    public class TableController : Controller
    {
        private readonly ITableLogic _tableLogic;

        /// <summary>
        /// Controller for handling operations related to tables.
        /// </summary>
        public TableController(ITableLogic tableLogic)
        {
            _tableLogic = tableLogic;
        }

        /// <summary>
        /// Retrieves logs from a table within a specified date range.
        /// </summary>
        /// <param name="fromDate">The starting date of the range.</param>
        /// <param name="toDate">The ending date of the range.</param>
        /// <returns>An action result containing the logs within the specified date range.</returns>
        [HttpGet(Name = "GetLogsInDateRange")]
        public async Task<ActionResult> GetLogsInDateRange(DateTime fromDate, DateTime toDate)
        {
            var logs = await _tableLogic.GetLogsInDateRange(fromDate, toDate, Tables.LogTable);
            return Ok(logs);
        }
    }
}
