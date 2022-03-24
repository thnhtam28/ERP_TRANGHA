using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class AdviseCardRepository : GenericRepository<ErpSaleDbContext, AdviseCard>, IAdviseCardRepository
    {
        public AdviseCardRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all AdviseCard
        /// </summary>
        /// <returns>AdviseCard list</returns>
        public IQueryable<AdviseCard> GetAllAdviseCard()
        {
            return Context.AdviseCard.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwAdviseCard> GetvwAllAdviseCard()
        {
            return Context.vwAdviseCard.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get AdviseCard information by specific id
        /// </summary>
        /// <param name="AdviseCardId">Id of AdviseCard</param>
        /// <returns></returns>
        public vwAdviseCard GetvwAdviseCardById(int Id)
        {
            return Context.vwAdviseCard.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public AdviseCard GetAdviseCardById(int Id)
        {
            return Context.AdviseCard.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert AdviseCard into database
        /// </summary>
        /// <param name="AdviseCard">Object infomation</param>
        public void InsertAdviseCard(AdviseCard AdviseCard)
        {
            Context.AdviseCard.Add(AdviseCard);
            Context.Entry(AdviseCard).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete AdviseCard with specific id
        /// </summary>
        /// <param name="Id">AdviseCard Id</param>
        public void DeleteAdviseCard(int Id)
        {
            AdviseCard deletedAdviseCard = GetAdviseCardById(Id);
            Context.AdviseCard.Remove(deletedAdviseCard);
            Context.Entry(deletedAdviseCard).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a AdviseCard with its Id and Update IsDeleted IF that AdviseCard has relationship with others
        /// </summary>
        /// <param name="AdviseCardId">Id of AdviseCard</param>
        public void DeleteAdviseCardRs(int Id)
        {
            AdviseCard deleteAdviseCardRs = GetAdviseCardById(Id);
            deleteAdviseCardRs.IsDeleted = true;
            UpdateAdviseCard(deleteAdviseCardRs);
        }

        /// <summary>
        /// Update AdviseCard into database
        /// </summary>
        /// <param name="AdviseCard">AdviseCard object</param>
        public void UpdateAdviseCard(AdviseCard AdviseCard)
        {
            Context.Entry(AdviseCard).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
