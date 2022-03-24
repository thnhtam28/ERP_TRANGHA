using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IDonateProOrSerRepository
    {
        /// <summary>
        /// Get all DonateProOrSer
        /// </summary>
        /// <returns>DonateProOrSer list</returns>
        IQueryable<DonateProOrSer> GetAllDonateProOrSer();
        IQueryable<vwDonateProOrSer> GetvwAllDonateProOrSer();
        /// <summary>
        /// Get DonateProOrSer information by specific id
        /// </summary>
        /// <param name="Id">Id of DonateProOrSer</param>
        /// <returns></returns>
        DonateProOrSer GetDonateProOrSerById(int Id);
        vwDonateProOrSer GetvwDonateProOrSerById(int Id);
        /// <summary>
        /// Insert DonateProOrSer into database
        /// </summary>
        /// <param name="DonateProOrSer">Object infomation</param>
        void InsertDonateProOrSer(DonateProOrSer DonateProOrSer);

        /// <summary>
        /// Delete DonateProOrSer with specific id
        /// </summary>
        /// <param name="Id">DonateProOrSer Id</param>
        void DeleteDonateProOrSer(int Id);

        /// <summary>
        /// Delete a DonateProOrSer with its Id and Update IsDeleted IF that DonateProOrSer has relationship with others
        /// </summary>
        /// <param name="Id">Id of DonateProOrSer</param>
        void DeleteDonateProOrSerRs(int Id);

        /// <summary>
        /// Update DonateProOrSer into database
        /// </summary>
        /// <param name="DonateProOrSer">DonateProOrSer object</param>
        void UpdateDonateProOrSer(DonateProOrSer DonateProOrSer);
    }
}
