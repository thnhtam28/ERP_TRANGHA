using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class AdviseCardDetailRepository : GenericRepository<ErpSaleDbContext, AdviseCardDetail>, IAdviseCardDetailRepository
    {
        public AdviseCardDetailRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all AdviseCardDetail
        /// </summary>
        /// <returns>AdviseCardDetail list</returns>
        public IQueryable<AdviseCardDetail> GetAllAdviseCardDetail()
        {
            return Context.AdviseCardDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<AdviseCardDetail> GetAllAdviseCardDetailById(int AdviseCardId)
        {
            return Context.AdviseCardDetail.Where(item => item.AdviseCardId == AdviseCardId && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get AdviseCardDetail information by specific id
        /// </summary>
        /// <param name="AdviseCardDetailId">Id of AdviseCardDetail</param>
        /// <returns></returns>
        public AdviseCardDetail GetAdviseCardDetailById(int Id)
        {
            return Context.AdviseCardDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public AdviseCardDetail GetAdviseCardDetailById(int AdviseCardId, int QuestionId, int AnswerId)
        {
            return Context.AdviseCardDetail.SingleOrDefault(item => item.AdviseCardId == AdviseCardId && item.TargetId==AnswerId && item.QuestionId==QuestionId && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert AdviseCardDetail into database
        /// </summary>
        /// <param name="AdviseCardDetail">Object infomation</param>
        public void InsertAdviseCardDetail(AdviseCardDetail AdviseCardDetail)
        {
            Context.AdviseCardDetail.Add(AdviseCardDetail);
            Context.Entry(AdviseCardDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete AdviseCardDetail with specific id
        /// </summary>
        /// <param name="Id">AdviseCardDetail Id</param>
        public void DeleteAdviseCardDetail(int Id)
        {
            AdviseCardDetail deletedAdviseCardDetail = GetAdviseCardDetailById(Id);
            Context.AdviseCardDetail.Remove(deletedAdviseCardDetail);
            Context.Entry(deletedAdviseCardDetail).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a AdviseCardDetail with its Id and Update IsDeleted IF that AdviseCardDetail has relationship with others
        /// </summary>
        /// <param name="AdviseCardDetailId">Id of AdviseCardDetail</param>
        public void DeleteAdviseCardDetailRs(int Id)
        {
            AdviseCardDetail deleteAdviseCardDetailRs = GetAdviseCardDetailById(Id);
            deleteAdviseCardDetailRs.IsDeleted = true;
            UpdateAdviseCardDetail(deleteAdviseCardDetailRs);
        }

        /// <summary>
        /// Update AdviseCardDetail into database
        /// </summary>
        /// <param name="AdviseCardDetail">AdviseCardDetail object</param>
        public void UpdateAdviseCardDetail(AdviseCardDetail AdviseCardDetail)
        {
            Context.Entry(AdviseCardDetail).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
