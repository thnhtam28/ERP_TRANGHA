using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface ILogEquipmentRepository
    {
        /// <summary>
        /// Get all LogEquipment
        /// </summary>
        /// <returns>LogEquipment list</returns>
        IQueryable<LogEquipment> GetAllLogEquipment();
        List<LogEquipment> GetListAllLogEquipment();
        IQueryable<vwLogEquipment> GetvwAllLogEquipment();
        /// <summary>
        /// Get LogEquipment information by specific id
        /// </summary>
        /// <param name="Id">Id of LogEquipment</param>
        /// <returns></returns>
        LogEquipment GetLogEquipmentById(int Id);
        vwLogEquipment GetvwLogEquipmentById(int Id);
        /// <summary>
        /// Insert LogEquipment into database
        /// </summary>
        /// <param name="LogEquipment">Object infomation</param>
        void InsertLogEquipment(LogEquipment LogEquipment);

        /// <summary>
        /// Delete LogEquipment with specific id
        /// </summary>
        /// <param name="Id">LogEquipment Id</param>
        void DeleteLogEquipment(int Id);

        /// <summary>
        /// Delete a LogEquipment with its Id and Update IsDeleted IF that LogEquipment has relationship with others
        /// </summary>
        /// <param name="Id">Id of LogEquipment</param>
        void DeleteLogEquipmentRs(int Id);

        /// <summary>
        /// Update LogEquipment into database
        /// </summary>
        /// <param name="LogEquipment">LogEquipment object</param>
        void UpdateLogEquipment(LogEquipment LogEquipment);
    }
}
