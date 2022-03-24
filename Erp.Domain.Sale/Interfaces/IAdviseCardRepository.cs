using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IAdviseCardRepository
    {
        /// <summary>
        /// Get all AdviseCard
        /// </summary>
        /// <returns>AdviseCard list</returns>
        IQueryable<AdviseCard> GetAllAdviseCard();
        IQueryable<vwAdviseCard> GetvwAllAdviseCard();
        /// <summary>
        /// Get AdviseCard information by specific id
        /// </summary>
        /// <param name="Id">Id of AdviseCard</param>
        /// <returns></returns>
        AdviseCard GetAdviseCardById(int Id);
        vwAdviseCard GetvwAdviseCardById(int Id);
        /// <summary>
        /// Insert AdviseCard into database
        /// </summary>
        /// <param name="AdviseCard">Object infomation</param>
        void InsertAdviseCard(AdviseCard AdviseCard);

        /// <summary>
        /// Delete AdviseCard with specific id
        /// </summary>
        /// <param name="Id">AdviseCard Id</param>
        void DeleteAdviseCard(int Id);

        /// <summary>
        /// Delete a AdviseCard with its Id and Update IsDeleted IF that AdviseCard has relationship with others
        /// </summary>
        /// <param name="Id">Id of AdviseCard</param>
        void DeleteAdviseCardRs(int Id);

        /// <summary>
        /// Update AdviseCard into database
        /// </summary>
        /// <param name="AdviseCard">AdviseCard object</param>
        void UpdateAdviseCard(AdviseCard AdviseCard);
    }
}
