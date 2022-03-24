using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface ISchedulingHistoryRepository
    {
        /// <summary>
        /// Get all SchedulingHistory
        /// </summary>
        /// <returns>SchedulingHistory list</returns>
        IQueryable<SchedulingHistory> GetAllSchedulingHistory();
        IQueryable<vwSchedulingHistory> GetvwAllSchedulingHistory();
        List<vwSchedulingHistory> GetListvwAllSchedulingHistory();
        /// <summary>
        /// Get SchedulingHistory information by specific id
        /// </summary>
        /// <param name="Id">Id of SchedulingHistory</param>
        /// <returns></returns>
        SchedulingHistory GetSchedulingHistoryById(int Id);
        vwSchedulingHistory GetvwSchedulingHistoryById(int Id);
        /// <summary>
        /// Insert SchedulingHistory into database
        /// </summary>
        /// <param name="SchedulingHistory">Object infomation</param>
        void InsertSchedulingHistory(SchedulingHistory SchedulingHistory);

        /// <summary>
        /// Delete SchedulingHistory with specific id
        /// </summary>
        /// <param name="Id">SchedulingHistory Id</param>
        void DeleteSchedulingHistory(int Id);

        /// <summary>
        /// Delete a SchedulingHistory with its Id and Update IsDeleted IF that SchedulingHistory has relationship with others
        /// </summary>
        /// <param name="Id">Id of SchedulingHistory</param>
        void DeleteSchedulingHistoryRs(int Id);

        /// <summary>
        /// Update SchedulingHistory into database
        /// </summary>
        /// <param name="SchedulingHistory">SchedulingHistory object</param>
        void UpdateSchedulingHistory(SchedulingHistory SchedulingHistory);
    }
}
