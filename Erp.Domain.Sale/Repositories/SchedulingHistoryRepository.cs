using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class SchedulingHistoryRepository : GenericRepository<ErpSaleDbContext, SchedulingHistory>, ISchedulingHistoryRepository
    {
        public SchedulingHistoryRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all SchedulingHistory
        /// </summary>
        /// <returns>SchedulingHistory list</returns>
        public IQueryable<SchedulingHistory> GetAllSchedulingHistory()
        {
            return Context.SchedulingHistory.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwSchedulingHistory> GetvwAllSchedulingHistory()
        {
            return Context.vwSchedulingHistory.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public List<vwSchedulingHistory> GetListvwAllSchedulingHistory()
        {
            return Context.vwSchedulingHistory.Where(item => (item.IsDeleted == null || item.IsDeleted == false)).ToList();
        }
        /// <summary>
        /// Get SchedulingHistory information by specific id
        /// </summary>
        /// <param name="SchedulingHistoryId">Id of SchedulingHistory</param>
        /// <returns></returns>
        public SchedulingHistory GetSchedulingHistoryById(int Id)
        {
            return Context.SchedulingHistory.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwSchedulingHistory GetvwSchedulingHistoryById(int Id)
        {
            return Context.vwSchedulingHistory.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert SchedulingHistory into database
        /// </summary>
        /// <param name="SchedulingHistory">Object infomation</param>
        public void InsertSchedulingHistory(SchedulingHistory SchedulingHistory)
        {
            Context.SchedulingHistory.Add(SchedulingHistory);
            Context.Entry(SchedulingHistory).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete SchedulingHistory with specific id
        /// </summary>
        /// <param name="Id">SchedulingHistory Id</param>
        public void DeleteSchedulingHistory(int Id)
        {
            SchedulingHistory deletedSchedulingHistory = GetSchedulingHistoryById(Id);
            Context.SchedulingHistory.Remove(deletedSchedulingHistory);
            Context.Entry(deletedSchedulingHistory).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a SchedulingHistory with its Id and Update IsDeleted IF that SchedulingHistory has relationship with others
        /// </summary>
        /// <param name="SchedulingHistoryId">Id of SchedulingHistory</param>
        public void DeleteSchedulingHistoryRs(int Id)
        {
            SchedulingHistory deleteSchedulingHistoryRs = GetSchedulingHistoryById(Id);
            deleteSchedulingHistoryRs.IsDeleted = true;
            UpdateSchedulingHistory(deleteSchedulingHistoryRs);
        }

        /// <summary>
        /// Update SchedulingHistory into database
        /// </summary>
        /// <param name="SchedulingHistory">SchedulingHistory object</param>
        public void UpdateSchedulingHistory(SchedulingHistory SchedulingHistory)
        {
            Context.Entry(SchedulingHistory).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
