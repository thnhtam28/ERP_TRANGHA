using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IMaterialOutboundRepository
    {
        /// <summary>
        /// Get all MaterialOutbound
        /// </summary>
        /// <returns>MaterialOutbound list</returns>
        IQueryable<MaterialOutbound> GetAllMaterialOutbound();
        IQueryable<vwMaterialOutbound> GetAllvwMaterialOutbound();
        IQueryable<MaterialOutbound> GetAllMaterialOutboundFull();
        IQueryable<vwMaterialOutbound> GetAllvwMaterialOutboundFull();
        /// <summary>
        /// Get MaterialOutbound information by specific id
        /// </summary>
        /// <param name="Id">Id of MaterialOutbound</param>
        /// <returns></returns>
        MaterialOutbound GetMaterialOutboundById(int Id);
        vwMaterialOutbound GetvwMaterialOutboundById(int Id);
        //MaterialOutbound GetMaterialOutboundBySaleOrderId(int InvoiceId);
        vwMaterialOutbound GetvwMaterialOutboundFullById(int Id);
        vwMaterialOutbound GetvwMaterialOutboundByTransactionCode(string Code);
        /// <summary>
        /// Insert MaterialOutbound into database
        /// </summary>
        /// <param name="MaterialOutbound">Object infomation</param>
        void InsertMaterialOutbound(MaterialOutbound MaterialOutbound);

        /// <summary>
        /// Delete MaterialOutbound with specific id
        /// </summary>
        /// <param name="Id">MaterialOutbound Id</param>
        void DeleteMaterialOutbound(int Id);

        /// <summary>
        /// Delete a MaterialOutbound with its Id and Update IsDeleted IF that MaterialOutbound has relationship with others
        /// </summary>
        /// <param name="Id">Id of MaterialOutbound</param>
        void DeleteMaterialOutboundRs(int Id);

        /// <summary>
        /// Update MaterialOutbound into database
        /// </summary>
        /// <param name="MaterialOutbound">MaterialOutbound object</param>
        void UpdateMaterialOutbound(MaterialOutbound MaterialOutbound);

        // ---------------------------------------------------------------------
        // outbound detail

        IQueryable<MaterialOutboundDetail> GetAllMaterialOutboundDetailByOutboundId(int OutboundId);
        IQueryable<vwMaterialOutboundDetail> GetAllvwMaterialOutboundDetailByOutboundId(int OutboundId);

        IQueryable<vwMaterialOutboundDetail> GetAllvwMaterialOutboundDetailByMaterialId(int MaterialId);
        IQueryable<vwMaterialOutboundDetail> GetAllvwMaterialOutboundDetail();

        MaterialOutboundDetail GetMaterialOutboundDetailById(int Id);

        void InsertMaterialOutboundDetail(MaterialOutboundDetail MaterialOutboundDetail);

        void DeleteMaterialOutboundDetail(int Id);

        void DeleteMaterialOutboundDetailRs(int Id);

        void UpdateMaterialOutboundDetail(MaterialOutboundDetail MaterialOutboundDetail);
    }
}
