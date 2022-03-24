using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IServiceDetailRepository
    {
        /// <summary>
        /// Get all ServiceDetail
        /// </summary>
        /// <returns>ServiceDetail list</returns>
        IQueryable<ServiceDetail> GetAllServiceDetail();
        IQueryable<vwServiceDetail> GetvwAllServiceDetail();
        /// <summary>
        /// Get ServiceDetail information by specific id
        /// </summary>
        /// <param name="Id">Id of ServiceDetail</param>
        /// <returns></returns>
        ServiceDetail GetServiceDetailById(int Id);
        vwServiceDetail GetvwServiceDetailById(int Id);
        /// <summary>
        /// Insert ServiceDetail into database
        /// </summary>
        /// <param name="ServiceDetail">Object infomation</param>
        void InsertServiceDetail(ServiceDetail ServiceDetail);
        
        /// <summary>
        /// Delete ServiceDetail with specific id
        /// </summary>
        /// <param name="Id">ServiceDetail Id</param>
        void DeleteServiceDetail(int Id);

        /// <summary>
        /// Delete a ServiceDetail with its Id and Update IsDeleted IF that ServiceDetail has relationship with others
        /// </summary>
        /// <param name="Id">Id of ServiceDetail</param>
        void DeleteServiceDetailRs(int Id);

        /// <summary>
        /// Update ServiceDetail into database
        /// </summary>
        /// <param name="ServiceDetail">ServiceDetail object</param>
        void UpdateServiceDetail(ServiceDetail ServiceDetail);
    }
}
