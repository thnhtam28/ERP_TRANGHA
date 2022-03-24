using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IServiceStepsRepository
    {
        /// <summary>
        /// Get all ServiceSteps
        /// </summary>
        /// <returns>ServiceSteps list</returns>
        IQueryable<ServiceSteps> GetAllServiceSteps();

        /// <summary>
        /// Get ServiceSteps information by specific id
        /// </summary>
        /// <param name="Id">Id of ServiceSteps</param>
        /// <returns></returns>
        ServiceSteps GetServiceStepsById(int Id);

        /// <summary>
        /// Insert ServiceSteps into database
        /// </summary>
        /// <param name="ServiceSteps">Object infomation</param>
        void InsertServiceSteps(ServiceSteps ServiceSteps);
        void InsertServiceDetail(ServiceDetail ServiceDetail);

        /// <summary>
        /// Delete ServiceSteps with specific id
        /// </summary>
        /// <param name="Id">ServiceSteps Id</param>
        void DeleteServiceSteps(int Id);

        /// <summary>
        /// Delete a ServiceSteps with its Id and Update IsDeleted IF that ServiceSteps has relationship with others
        /// </summary>
        /// <param name="Id">Id of ServiceSteps</param>
        void DeleteServiceStepsRs(int Id);

        /// <summary>
        /// Update ServiceSteps into database
        /// </summary>
        /// <param name="ServiceSteps">ServiceSteps object</param>
        void UpdateServiceSteps(ServiceSteps ServiceSteps);
    }
}
