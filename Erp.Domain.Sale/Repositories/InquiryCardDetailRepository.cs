using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class InquiryCardDetailRepository : GenericRepository<ErpSaleDbContext, InquiryCardDetail>, IInquiryCardDetailRepository
    {
        public InquiryCardDetailRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all InquiryCardDetail
        /// </summary>
        /// <returns>InquiryCardDetail list</returns>
        public IQueryable<InquiryCardDetail> GetAllInquiryCardDetail()
        {
            return Context.InquiryCardDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get InquiryCardDetail information by specific id
        /// </summary>
        /// <param name="InquiryCardDetailId">Id of InquiryCardDetail</param>
        /// <returns></returns>
        public InquiryCardDetail GetInquiryCardDetailById(int Id)
        {
            return Context.InquiryCardDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert InquiryCardDetail into database
        /// </summary>
        /// <param name="InquiryCardDetail">Object infomation</param>
        public void InsertInquiryCardDetail(InquiryCardDetail InquiryCardDetail)
        {
            Context.InquiryCardDetail.Add(InquiryCardDetail);
            Context.Entry(InquiryCardDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete InquiryCardDetail with specific id
        /// </summary>
        /// <param name="Id">InquiryCardDetail Id</param>
        public void DeleteInquiryCardDetail(int Id)
        {
            InquiryCardDetail deletedInquiryCardDetail = GetInquiryCardDetailById(Id);
            Context.InquiryCardDetail.Remove(deletedInquiryCardDetail);
            Context.Entry(deletedInquiryCardDetail).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a InquiryCardDetail with its Id and Update IsDeleted IF that InquiryCardDetail has relationship with others
        /// </summary>
        /// <param name="InquiryCardDetailId">Id of InquiryCardDetail</param>
        public void DeleteInquiryCardDetailRs(int Id)
        {
            InquiryCardDetail deleteInquiryCardDetailRs = GetInquiryCardDetailById(Id);
            deleteInquiryCardDetailRs.IsDeleted = true;
            UpdateInquiryCardDetail(deleteInquiryCardDetailRs);
        }

        /// <summary>
        /// Update InquiryCardDetail into database
        /// </summary>
        /// <param name="InquiryCardDetail">InquiryCardDetail object</param>
        public void UpdateInquiryCardDetail(InquiryCardDetail InquiryCardDetail)
        {
            Context.Entry(InquiryCardDetail).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
