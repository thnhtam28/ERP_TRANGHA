using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IMaterialInboundRepository
    {
        /// <summary>
        /// Get all MaterialInbound
        /// </summary>
        /// <returns>MaterialInbound list</returns>
        IQueryable<MaterialInbound> GetAllMaterialInbound();
        IQueryable<vwMaterialInbound> GetAllvwMaterialInbound();
        IQueryable<vwMaterialInbound> GetAllvwMaterialInboundFull();
        /// <summary>
        /// Get MaterialInbound information by specific id
        /// </summary>
        /// <param name="Id">Id of MaterialInbound</param>
        /// <returns></returns>
        MaterialInbound GetMaterialInboundById(int Id);
        vwMaterialInbound GetvwMaterialInboundById(int Id);
        vwMaterialInbound GetvwMaterialInboundByTransactionCode(string Code);
        vwMaterialInbound GetvwMaterialInboundFullById(int Id);
        /// <summary>
        /// Insert MaterialInbound into database
        /// </summary>
        /// <param name="MaterialInbound">Object infomation</param>
        void InsertMaterialInbound(MaterialInbound MaterialInbound);

        /// <summary>
        /// Delete MaterialInbound with specific id
        /// </summary>
        /// <param name="Id">MaterialInbound Id</param>
        void DeleteMaterialInbound(int Id);

        /// <summary>
        /// Delete a MaterialInbound with its Id and Update IsDeleted IF that MaterialInbound has relationship with others
        /// </summary>
        /// <param name="Id">Id of MaterialInbound</param>
        void DeleteMaterialInboundRs(int Id);

        /// <summary>
        /// Update MaterialInbound into database
        /// </summary>
        /// <param name="MaterialInbound">MaterialInbound object</param>
        void UpdateMaterialInbound(MaterialInbound MaterialInbound);


        //----------------------------------------------------------------------------------------
        // inbound detail

        IQueryable<MaterialInboundDetail> GetAllMaterialInboundDetailByInboundId(int inboundId);

        IQueryable<vwMaterialInboundDetail> GetAllvwMaterialInboundDetailByInboundId(int inboundId);

        IQueryable<vwMaterialInboundDetail> GetAllvwMaterialInboundDetailByMaterialId(int MaterialId);
        IQueryable<vwMaterialInboundDetail> GetAllvwMaterialInboundDetail();

        MaterialInboundDetail GetMaterialInboundDetailById(int Id);

        void InsertMaterialInboundDetail(MaterialInboundDetail MaterialInBoundDetail);

        void DeleteMaterialInboundDetail(int Id);

        void DeleteMaterialInboundDetailRs(int Id);

        void UpdateMaterialInboundDetail(MaterialInboundDetail MaterialInBoundDetail);
    }
}
