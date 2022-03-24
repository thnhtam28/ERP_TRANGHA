using Erp.Domain.Account.Entities;
using Erp.Domain.Account.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Account.Repositories
{
    public class LogReceiptRepository : GenericRepository<ErpAccountDbContext, LogReceipt>, ILogReceiptRepository
    {
        public LogReceiptRepository(ErpAccountDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all LogReceipt
        /// </summary>
        /// <returns>LogReceipt list</returns>
        public IQueryable<vwLogReceipt> GetAllLogReceipt()
        {
            return Context.vwLogReceipt.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get LogReceipt information by specific id
        /// </summary>
        /// <param name="LogReceiptId">Id of LogReceipt</param>
        /// <returns></returns>
        public LogReceipt GetLogReceiptById(int Id)
        {
            return Context.LogReceipt.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert LogReceipt into database
        /// </summary>
        /// <param name="LogReceipt">Object infomation</param>
        public void InsertLogReceipt(LogReceipt LogReceipt)
        {
            Context.LogReceipt.Add(LogReceipt);
            Context.Entry(LogReceipt).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete LogReceipt with specific id
        /// </summary>
        /// <param name="Id">LogReceipt Id</param>
        public void DeleteLogReceipt(int Id)
        {
            LogReceipt deletedLogReceipt = GetLogReceiptById(Id);
            Context.LogReceipt.Remove(deletedLogReceipt);
            Context.Entry(deletedLogReceipt).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a LogReceipt with its Id and Update IsDeleted IF that LogReceipt has relationship with others
        /// </summary>
        /// <param name="LogReceiptId">Id of LogReceipt</param>
        public void DeleteLogReceiptRs(int Id)
        {
            LogReceipt deleteLogReceiptRs = GetLogReceiptById(Id);
            deleteLogReceiptRs.IsDeleted = true;
            UpdateLogReceipt(deleteLogReceiptRs);
        }

        /// <summary>
        /// Update LogReceipt into database
        /// </summary>
        /// <param name="LogReceipt">LogReceipt object</param>
        public void UpdateLogReceipt(LogReceipt LogReceipt)
        {
            Context.Entry(LogReceipt).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
