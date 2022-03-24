using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class ServiceDetailRepository : GenericRepository<ErpSaleDbContext, ServiceDetail>, IServiceDetailRepository
    {
        public ServiceDetailRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all ServiceDetail
        /// </summary>
        /// <returns>ServiceDetail list</returns>
        public IQueryable<ServiceDetail> GetAllServiceDetail()
        {
            return Context.ServiceDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwServiceDetail> GetvwAllServiceDetail()
        {
            return Context.vwServiceDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get ServiceDetail information by specific id
        /// </summary>
        /// <param name="ServiceDetailId">Id of ServiceDetail</param>
        /// <returns></returns>
        public ServiceDetail GetServiceDetailById(int Id)
        {
            return Context.ServiceDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwServiceDetail GetvwServiceDetailById(int Id)
        {
            return Context.vwServiceDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert ServiceDetail into database
        /// </summary>
        /// <param name="ServiceDetail">Object infomation</param>
        public void InsertServiceDetail(ServiceDetail ServiceDetail)
        {
            Context.ServiceDetail.Add(ServiceDetail);
            Context.Entry(ServiceDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete ServiceDetail with specific id
        /// </summary>
        /// <param name="Id">ServiceDetail Id</param>
        public void DeleteServiceDetail(int Id)
        {
            ServiceDetail deletedServiceDetail = GetServiceDetailById(Id);
            Context.ServiceDetail.Remove(deletedServiceDetail);
            Context.Entry(deletedServiceDetail).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a ServiceDetail with its Id and Update IsDeleted IF that ServiceDetail has relationship with others
        /// </summary>
        /// <param name="ServiceDetailId">Id of ServiceDetail</param>
        public void DeleteServiceDetailRs(int Id)
        {
            ServiceDetail deleteServiceDetailRs = GetServiceDetailById(Id);
            deleteServiceDetailRs.IsDeleted = true;
            UpdateServiceDetail(deleteServiceDetailRs);
        }

        /// <summary>
        /// Update ServiceDetail into database
        /// </summary>
        /// <param name="ServiceDetail">ServiceDetail object</param>
        public void UpdateServiceDetail(ServiceDetail ServiceDetail)
        {
            Context.Entry(ServiceDetail).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
