using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IStaffMadeRepository
    {
        /// <summary>
        /// Get all StaffMade
        /// </summary>
        /// <returns>StaffMade list</returns>
        IQueryable<StaffMade> GetAllStaffMade();
        IQueryable<vwStaffMade> GetvwAllStaffMade();
        /// <summary>
        /// Get StaffMade information by specific id
        /// </summary>
        /// <param name="Id">Id of StaffMade</param>
        /// <returns></returns>
        StaffMade GetStaffMadeById(int Id);
        vwStaffMade GetvwStaffMadeById(int Id);
        /// <summary>
        /// Insert StaffMade into database
        /// </summary>
        /// <param name="StaffMade">Object infomation</param>
        void InsertStaffMade(StaffMade StaffMade);

        /// <summary>
        /// Delete StaffMade with specific id
        /// </summary>
        /// <param name="Id">StaffMade Id</param>
        void DeleteStaffMade(int Id);

        /// <summary>
        /// Delete a StaffMade with its Id and Update IsDeleted IF that StaffMade has relationship with others
        /// </summary>
        /// <param name="Id">Id of StaffMade</param>
        void DeleteStaffMadeRs(int Id);

        /// <summary>
        /// Update StaffMade into database
        /// </summary>
        /// <param name="StaffMade">StaffMade object</param>
        void UpdateStaffMade(StaffMade StaffMade);
    }
}
