using Erp.Domain.Account.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Account.Interfaces
{
    public interface ILogReceiptRepository
    {
        /// <summary>
        /// Get all LogReceipt
        /// </summary>
        /// <returns>LogReceipt list</returns>
        IQueryable<vwLogReceipt> GetAllLogReceipt();

        /// <summary>
        /// Get LogReceipt information by specific id
        /// </summary>
        /// <param name="Id">Id of LogReceipt</param>
        /// <returns></returns>
        LogReceipt GetLogReceiptById(int Id);

        /// <summary>
        /// Insert LogReceipt into database
        /// </summary>
        /// <param name="LogReceipt">Object infomation</param>
        void InsertLogReceipt(LogReceipt LogReceipt);

        /// <summary>
        /// Delete LogReceipt with specific id
        /// </summary>
        /// <param name="Id">LogReceipt Id</param>
        void DeleteLogReceipt(int Id);

        /// <summary>
        /// Delete a LogReceipt with its Id and Update IsDeleted IF that LogReceipt has relationship with others
        /// </summary>
        /// <param name="Id">Id of LogReceipt</param>
        void DeleteLogReceiptRs(int Id);

        /// <summary>
        /// Update LogReceipt into database
        /// </summary>
        /// <param name="LogReceipt">LogReceipt object</param>
        void UpdateLogReceipt(LogReceipt LogReceipt);
    }
}
