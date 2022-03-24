using Erp.Domain.Crm.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Crm.Interfaces
{
    public interface ICALLogRepository
    {
        /// <summary>
        /// Get all CALLog
        /// </summary>
        /// <returns>CALLog list</returns>
        IQueryable<CALLog> GetAllCALLog();

        /// <summary>
        /// Get CALLog information by specific id
        /// </summary>
        /// <param name="Id">Id of CALLog</param>
        /// <returns></returns>
        CALLog GetCALLogById(int Id);

        CALLog GetCALLogByKeyLog(string keylog);

        /// <summary>
        /// Insert CALLog into database
        /// </summary>
        /// <param name="CALLog">Object infomation</param>
        void InsertCALLog(CALLog CALLog);

        /// <summary>
        /// Delete CALLog with specific id
        /// </summary>
        /// <param name="Id">CALLog Id</param>
        void DeleteCALLog(int Id);

        /// <summary>
        /// Delete a CALLog with its Id and Update IsDeleted IF that CALLog has relationship with others
        /// </summary>
        /// <param name="Id">Id of CALLog</param>
        void DeleteCALLogRs(int Id);

        /// <summary>
        /// Update CALLog into database
        /// </summary>
        /// <param name="CALLog">CALLog object</param>
        void UpdateCALLog(CALLog CALLog);
    }
}
