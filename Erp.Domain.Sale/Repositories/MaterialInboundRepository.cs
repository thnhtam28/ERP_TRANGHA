using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class MaterialInboundRepository : GenericRepository<ErpSaleDbContext, MaterialInbound>, IMaterialInboundRepository
    {
        public MaterialInboundRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all MaterialInbound
        /// </summary>
        /// <returns>MaterialInbound list</returns>
        public IQueryable<MaterialInbound> GetAllMaterialInbound()
        {
            return Context.MaterialInbound.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwMaterialInbound> GetAllvwMaterialInbound()
        {
            return Context.vwMaterialInbound.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwMaterialInbound> GetAllvwMaterialInboundFull()
        {
            return Context.vwMaterialInbound;
        }
        /// <summary>
        /// Get MaterialInbound information by specific id
        /// </summary>
        /// <param name="MaterialInboundId">Id of MaterialInbound</param>
        /// <returns></returns>
        public MaterialInbound GetMaterialInboundById(int Id)
        {
            return Context.MaterialInbound.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public vwMaterialInbound GetvwMaterialInboundByTransactionCode(string TransactionCode)
        {
            return Context.vwMaterialInbound.SingleOrDefault(item => item.Code == TransactionCode);
        }
        public vwMaterialInbound GetvwMaterialInboundById(int Id)
        {
            return Context.vwMaterialInbound.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwMaterialInbound GetvwMaterialInboundFullById(int Id)
        {
            return Context.vwMaterialInbound.SingleOrDefault(item => item.Id == Id);
        }
        /// <summary>
        /// Insert MaterialInbound into database
        /// </summary>
        /// <param name="MaterialInbound">Object infomation</param>
        public void InsertMaterialInbound(MaterialInbound MaterialInbound)
        {
            Context.MaterialInbound.Add(MaterialInbound);
            Context.Entry(MaterialInbound).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete MaterialInbound with specific id
        /// </summary>
        /// <param name="Id">MaterialInbound Id</param>
        public void DeleteMaterialInbound(int Id)
        {
            MaterialInbound deletedMaterialInbound = GetMaterialInboundById(Id);
            Context.MaterialInbound.Remove(deletedMaterialInbound);
            Context.Entry(deletedMaterialInbound).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a MaterialInbound with its Id and Update IsDeleted IF that MaterialInbound has relationship with others
        /// </summary>
        /// <param name="MaterialInboundId">Id of MaterialInbound</param>
        public void DeleteMaterialInboundRs(int Id)
        {
            MaterialInbound deleteMaterialInboundRs = GetMaterialInboundById(Id);
            deleteMaterialInboundRs.IsDeleted = true;
            UpdateMaterialInbound(deleteMaterialInboundRs);
        }

        /// <summary>
        /// Update MaterialInbound into database
        /// </summary>
        /// <param name="MaterialInbound">MaterialInbound object</param>
        public void UpdateMaterialInbound(MaterialInbound MaterialInbound)
        {
            Context.Entry(MaterialInbound).State = EntityState.Modified;
            Context.SaveChanges();
        }


        //----------------------------------------------------------------------------------------
        // Inbound detail

        public IQueryable<MaterialInboundDetail> GetAllMaterialInboundDetailByInboundId(int inboundId)
        {
            return Context.MaterialInboundDetail.Where(item => item.MaterialInboundId == inboundId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwMaterialInboundDetail> GetAllvwMaterialInboundDetailByInboundId(int inboundId)
        {
            return Context.vwMaterialInboundDetail.Where(item => item.MaterialInboundId == inboundId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwMaterialInboundDetail> GetAllvwMaterialInboundDetailByMaterialId(int MaterialId)
        {
            return Context.vwMaterialInboundDetail.Where(item => item.MaterialId == MaterialId && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwMaterialInboundDetail> GetAllvwMaterialInboundDetail()
        {
            return Context.vwMaterialInboundDetail.Where(item =>item.IsDeleted == null || item.IsDeleted == false);
        }
        public MaterialInboundDetail GetMaterialInboundDetailById(int Id)
        {
            return Context.MaterialInboundDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertMaterialInboundDetail(MaterialInboundDetail MaterialInboundDetail)
        {
            Context.MaterialInboundDetail.Add(MaterialInboundDetail);
            Context.Entry(MaterialInboundDetail).State = EntityState.Added;
            Context.SaveChanges();
        }


        public void DeleteMaterialInboundDetail(int Id)
        {
            MaterialInboundDetail deletedMaterialInboundDetail = GetMaterialInboundDetailById(Id);
            Context.MaterialInboundDetail.Remove(deletedMaterialInboundDetail);
            Context.Entry(deletedMaterialInboundDetail).State = EntityState.Deleted;
            Context.SaveChanges();
        }


        public void DeleteMaterialInboundDetailRs(int Id)
        {
            MaterialInboundDetail deleteMaterialInboundDetailRs = GetMaterialInboundDetailById(Id);
            deleteMaterialInboundDetailRs.IsDeleted = true;
            UpdateMaterialInboundDetail(deleteMaterialInboundDetailRs);
        }


        public void UpdateMaterialInboundDetail(MaterialInboundDetail MaterialInboundDetail)
        {
            Context.Entry(MaterialInboundDetail).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
