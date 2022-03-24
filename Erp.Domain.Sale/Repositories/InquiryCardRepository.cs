using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class InquiryCardRepository : GenericRepository<ErpSaleDbContext, InquiryCard>, IInquiryCardRepository
    {
        public InquiryCardRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all InquiryCard
        /// </summary>
        /// <returns>InquiryCard list</returns>
        public IQueryable<InquiryCard> GetAllInquiryCard()
        {
            return Context.InquiryCard.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwInquiryCard> GetvwAllInquiryCard()
        {
            return Context.vwInquiryCard.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get InquiryCard information by specific id
        /// </summary>
        /// <param name="InquiryCardId">Id of InquiryCard</param>
        /// <returns></returns>
        public InquiryCard GetInquiryCardById(int Id)
        {
            return Context.InquiryCard.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwInquiryCard GetvwInquiryCardById(int Id)
        {
            return Context.vwInquiryCard.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwInquiryCard GetvwInquiryCardByCode(string code)
        {
            return Context.vwInquiryCard.SingleOrDefault(item => item.Code == code && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert InquiryCard into database
        /// </summary>
        /// <param name="InquiryCard">Object infomation</param>
        public void InsertInquiryCard(InquiryCard InquiryCard)
        {
            Context.InquiryCard.Add(InquiryCard);
            Context.Entry(InquiryCard).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete InquiryCard with specific id
        /// </summary>
        /// <param name="Id">InquiryCard Id</param>
        public void DeleteInquiryCard(int Id)
        {
            InquiryCard deletedInquiryCard = GetInquiryCardById(Id);
            Context.InquiryCard.Remove(deletedInquiryCard);
            Context.Entry(deletedInquiryCard).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a InquiryCard with its Id and Update IsDeleted IF that InquiryCard has relationship with others
        /// </summary>
        /// <param name="InquiryCardId">Id of InquiryCard</param>
        public void DeleteInquiryCardRs(int Id)
        {
            InquiryCard deleteInquiryCardRs = GetInquiryCardById(Id);
            deleteInquiryCardRs.IsDeleted = true;
            UpdateInquiryCard(deleteInquiryCardRs);
        }

        /// <summary>
        /// Update InquiryCard into database
        /// </summary>
        /// <param name="InquiryCard">InquiryCard object</param>
        public void UpdateInquiryCard(InquiryCard InquiryCard)
        {
            Context.Entry(InquiryCard).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
