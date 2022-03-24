using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface ICommisionInvoiceRepository
    {
        /// <summary>
        /// Get all CommisionInvoice
        /// </summary>
        /// <returns>CommisionInvoice list</returns>
        IQueryable<CommisionInvoice> GetAllCommisionInvoice();

        /// <summary>
        /// Get CommisionInvoice information by specific id
        /// </summary>
        /// <param name="Id">Id of CommisionInvoice</param>
        /// <returns></returns>
        CommisionInvoice GetCommisionInvoiceById(int Id);

        /// <summary>
        /// Insert CommisionInvoice into database
        /// </summary>
        /// <param name="CommisionInvoice">Object infomation</param>
        void InsertCommisionInvoice(CommisionInvoice CommisionInvoice);

        /// <summary>
        /// Delete CommisionInvoice with specific id
        /// </summary>
        /// <param name="Id">CommisionInvoice Id</param>
        void DeleteCommisionInvoice(int Id);

        /// <summary>
        /// Delete a CommisionInvoice with its Id and Update IsDeleted IF that CommisionInvoice has relationship with others
        /// </summary>
        /// <param name="Id">Id of CommisionInvoice</param>
        void DeleteCommisionInvoiceRs(int Id);

        /// <summary>
        /// Update CommisionInvoice into database
        /// </summary>
        /// <param name="CommisionInvoice">CommisionInvoice object</param>
        void UpdateCommisionInvoice(CommisionInvoice CommisionInvoice);
    }
}
