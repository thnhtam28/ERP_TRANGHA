using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class MaterialOutboundRepository : GenericRepository<ErpSaleDbContext, MaterialOutbound>, IMaterialOutboundRepository
    {
        public MaterialOutboundRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all MaterialOutbound
        /// </summary>
        /// <returns>MaterialOutbound list</returns>
        public IQueryable<MaterialOutbound> GetAllMaterialOutbound()
        {
            return Context.MaterialOutbound.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<MaterialOutbound> GetAllMaterialOutboundFull()
        {
            return Context.MaterialOutbound;
        }
        public IQueryable<vwMaterialOutbound> GetAllvwMaterialOutbound()
        {
            return Context.vwMaterialOutbound.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwMaterialOutbound> GetAllvwMaterialOutboundFull()
        {
            return Context.vwMaterialOutbound;
        }
        /// <summary>
        /// Get MaterialOutbound information by specific id
        /// </summary>
        /// <param name="MaterialOutboundId">Id of MaterialOutbound</param>
        /// <returns></returns>
        public MaterialOutbound GetMaterialOutboundById(int Id)
        {
            return Context.MaterialOutbound.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwMaterialOutbound GetvwMaterialOutboundById(int Id)
        {
            return Context.vwMaterialOutbound.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwMaterialOutbound GetvwMaterialOutboundFullById(int Id)
        {
            return Context.vwMaterialOutbound.SingleOrDefault(item => item.Id == Id);
        }
        public vwMaterialOutbound GetvwMaterialOutboundByTransactionCode(string TransactionCode)
        {
            return Context.vwMaterialOutbound.SingleOrDefault(item => item.Code == TransactionCode);
        }
        //public MaterialOutbound GetMaterialOutboundBySaleOrderId(int SaleOrderId)
        //{
        //    return Context.MaterialOutbound.SingleOrDefault(item => item.InvoiceId == SaleOrderId && (item.IsDeleted == null || item.IsDeleted == false));
        //}

        /// <summary>
        /// Insert MaterialOutbound into database
        /// </summary>
        /// <param name="MaterialOutbound">Object infomation</param>
        public void InsertMaterialOutbound(MaterialOutbound MaterialOutbound)
        {
            Context.MaterialOutbound.Add(MaterialOutbound);
            Context.Entry(MaterialOutbound).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete MaterialOutbound with specific id
        /// </summary>
        /// <param name="Id">MaterialOutbound Id</param>
        public void DeleteMaterialOutbound(int Id)
        {
            MaterialOutbound deletedMaterialOutbound = GetMaterialOutboundById(Id);
            Context.MaterialOutbound.Remove(deletedMaterialOutbound);
            Context.Entry(deletedMaterialOutbound).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a MaterialOutbound with its Id and Update IsDeleted IF that MaterialOutbound has relationship with others
        /// </summary>
        /// <param name="MaterialOutboundId">Id of MaterialOutbound</param>
        public void DeleteMaterialOutboundRs(int Id)
        {
            MaterialOutbound deleteMaterialOutboundRs = GetMaterialOutboundById(Id);
            deleteMaterialOutboundRs.IsDeleted = true;
            UpdateMaterialOutbound(deleteMaterialOutboundRs);
        }

        /// <summary>
        /// Update MaterialOutbound into database
        /// </summary>
        /// <param name="MaterialOutbound">MaterialOutbound object</param>
        public void UpdateMaterialOutbound(MaterialOutbound MaterialOutbound)
        {
            Context.Entry(MaterialOutbound).State = EntityState.Modified;
            Context.SaveChanges();
        }

        // --------------------------------------------------------------------------------
        //outbound detail

        public IQueryable<MaterialOutboundDetail> GetAllMaterialOutboundDetailByOutboundId(int OutboundId)
        {
            return Context.MaterialOutboundDetail.Where(item => item.MaterialOutboundId == OutboundId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwMaterialOutboundDetail> GetAllvwMaterialOutboundDetailByOutboundId(int OutboundId)
        {
            return Context.vwMaterialOutboundDetail.Where(item => item.MaterialOutboundId == OutboundId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwMaterialOutboundDetail> GetAllvwMaterialOutboundDetailByMaterialId(int MaterialId)
        {
            return Context.vwMaterialOutboundDetail.Where(item => item.MaterialId == MaterialId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public MaterialOutboundDetail GetMaterialOutboundDetailById(int Id)
        {
            return Context.MaterialOutboundDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwMaterialOutboundDetail> GetAllvwMaterialOutboundDetail()
        {
            return Context.vwMaterialOutboundDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertMaterialOutboundDetail(MaterialOutboundDetail MaterialOutboundDetail)
        {
            Context.MaterialOutboundDetail.Add(MaterialOutboundDetail);
            Context.Entry(MaterialOutboundDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void DeleteMaterialOutboundDetail(int Id)
        {
            MaterialOutboundDetail deletedMaterialOutboundDetail = GetMaterialOutboundDetailById(Id);
            Context.MaterialOutboundDetail.Remove(deletedMaterialOutboundDetail);
            Context.Entry(deletedMaterialOutboundDetail).State = EntityState.Deleted;
            Context.SaveChanges();
        }


        public void DeleteMaterialOutboundDetailRs(int Id)
        {
            MaterialOutboundDetail deleteMaterialOutboundDetailRs = GetMaterialOutboundDetailById(Id);
            deleteMaterialOutboundDetailRs.IsDeleted = true;
            UpdateMaterialOutboundDetail(deleteMaterialOutboundDetailRs);
        }


        public void UpdateMaterialOutboundDetail(MaterialOutboundDetail MaterialOutboundDetail)
        {
            Context.Entry(MaterialOutboundDetail).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
