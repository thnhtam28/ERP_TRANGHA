using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface ILogPromotionRepository
    {
        /// <summary>
        /// Get all LogPromotion
        /// </summary>
        /// <returns>LogPromotion list</returns>
        IQueryable<LogPromotion> GetAllLogPromotion();
        IQueryable<vwLogPromotion> GetvwAllLogPromotion();
        /// <summary>
        /// Get LogPromotion information by specific id
        /// </summary>
        /// <param name="Id">Id of LogPromotion</param>
        /// <returns></returns>
        LogPromotion GetLogPromotionById(int Id);
        vwLogPromotion GetvwLogPromotionById(int Id);
        /// <summary>
        /// Insert LogPromotion into database
        /// </summary>
        /// <param name="LogPromotion">Object infomation</param>
        void InsertLogPromotion(LogPromotion LogPromotion);

        /// <summary>
        /// Delete LogPromotion with specific id
        /// </summary>
        /// <param name="Id">LogPromotion Id</param>
        void DeleteLogPromotion(int Id);

        /// <summary>
        /// Delete a LogPromotion with its Id and Update IsDeleted IF that LogPromotion has relationship with others
        /// </summary>
        /// <param name="Id">Id of LogPromotion</param>
        void DeleteLogPromotionRs(int Id);

        /// <summary>
        /// Update LogPromotion into database
        /// </summary>
        /// <param name="LogPromotion">LogPromotion object</param>
        void UpdateLogPromotion(LogPromotion LogPromotion);
    }
}
