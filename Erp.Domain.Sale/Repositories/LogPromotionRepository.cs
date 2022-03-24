using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class LogPromotionRepository : GenericRepository<ErpSaleDbContext, LogPromotion>, ILogPromotionRepository
    {
        public LogPromotionRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all LogPromotion
        /// </summary>
        /// <returns>LogPromotion list</returns>
        public IQueryable<LogPromotion> GetAllLogPromotion()
        {
            return Context.LogPromotion.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwLogPromotion> GetvwAllLogPromotion()
        {
            return Context.vwLogPromotion.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get LogPromotion information by specific id
        /// </summary>
        /// <param name="LogPromotionId">Id of LogPromotion</param>
        /// <returns></returns>
        public LogPromotion GetLogPromotionById(int Id)
        {
            return Context.LogPromotion.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwLogPromotion GetvwLogPromotionById(int Id)
        {
            return Context.vwLogPromotion.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert LogPromotion into database
        /// </summary>
        /// <param name="LogPromotion">Object infomation</param>
        public void InsertLogPromotion(LogPromotion LogPromotion)
        {
            Context.LogPromotion.Add(LogPromotion);
            Context.Entry(LogPromotion).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete LogPromotion with specific id
        /// </summary>
        /// <param name="Id">LogPromotion Id</param>
        public void DeleteLogPromotion(int Id)
        {
            LogPromotion deletedLogPromotion = GetLogPromotionById(Id);
            Context.LogPromotion.Remove(deletedLogPromotion);
            Context.Entry(deletedLogPromotion).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a LogPromotion with its Id and Update IsDeleted IF that LogPromotion has relationship with others
        /// </summary>
        /// <param name="LogPromotionId">Id of LogPromotion</param>
        public void DeleteLogPromotionRs(int Id)
        {
            LogPromotion deleteLogPromotionRs = GetLogPromotionById(Id);
            deleteLogPromotionRs.IsDeleted = true;
            UpdateLogPromotion(deleteLogPromotionRs);
        }

        /// <summary>
        /// Update LogPromotion into database
        /// </summary>
        /// <param name="LogPromotion">LogPromotion object</param>
        public void UpdateLogPromotion(LogPromotion LogPromotion)
        {
            Context.Entry(LogPromotion).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
