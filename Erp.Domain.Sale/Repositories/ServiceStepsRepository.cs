using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class ServiceStepsRepository : GenericRepository<ErpSaleDbContext, ServiceSteps>, IServiceStepsRepository
    {
        public ServiceStepsRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all ServiceSteps
        /// </summary>
        /// <returns>ServiceSteps list</returns>
        public IQueryable<ServiceSteps> GetAllServiceSteps()
        {
            return Context.ServiceSteps.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get ServiceSteps information by specific id
        /// </summary>
        /// <param name="ServiceStepsId">Id of ServiceSteps</param>
        /// <returns></returns>
        public ServiceSteps GetServiceStepsById(int Id)
        {
            return Context.ServiceSteps.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert ServiceSteps into database
        /// </summary>
        /// <param name="ServiceSteps">Object infomation</param>
        public void InsertServiceSteps(ServiceSteps ServiceSteps)
        {
            Context.ServiceSteps.Add(ServiceSteps);
            Context.Entry(ServiceSteps).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void InsertServiceDetail(ServiceDetail ServiceDetail)
        {
            Context.ServiceDetail.Add(ServiceDetail);
            Context.Entry(ServiceDetail).State = EntityState.Added;
            Context.SaveChanges();
        }
        /// <summary>
        /// Delete ServiceSteps with specific id
        /// </summary>
        /// <param name="Id">ServiceSteps Id</param>
        public void DeleteServiceSteps(int Id)
        {
            ServiceSteps deletedServiceSteps = GetServiceStepsById(Id);
            Context.ServiceSteps.Remove(deletedServiceSteps);
            Context.Entry(deletedServiceSteps).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a ServiceSteps with its Id and Update IsDeleted IF that ServiceSteps has relationship with others
        /// </summary>
        /// <param name="ServiceStepsId">Id of ServiceSteps</param>
        public void DeleteServiceStepsRs(int Id)
        {
            ServiceSteps deleteServiceStepsRs = GetServiceStepsById(Id);
            deleteServiceStepsRs.IsDeleted = true;
            UpdateServiceSteps(deleteServiceStepsRs);
        }

        /// <summary>
        /// Update ServiceSteps into database
        /// </summary>
        /// <param name="ServiceSteps">ServiceSteps object</param>
        public void UpdateServiceSteps(ServiceSteps ServiceSteps)
        {
            Context.Entry(ServiceSteps).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
