using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class CommisionInvoiceRepository : GenericRepository<ErpSaleDbContext, CommisionInvoice>, ICommisionInvoiceRepository
    {
        public CommisionInvoiceRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all CommisionInvoice
        /// </summary>
        /// <returns>CommisionInvoice list</returns>
        public IQueryable<CommisionInvoice> GetAllCommisionInvoice()
        {
            return Context.CommisionInvoice.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get CommisionInvoice information by specific id
        /// </summary>
        /// <param name="CommisionInvoiceId">Id of CommisionInvoice</param>
        /// <returns></returns>
        public CommisionInvoice GetCommisionInvoiceById(int Id)
        {
            return Context.CommisionInvoice.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert CommisionInvoice into database
        /// </summary>
        /// <param name="CommisionInvoice">Object infomation</param>
        public void InsertCommisionInvoice(CommisionInvoice CommisionInvoice)
        {
            Context.CommisionInvoice.Add(CommisionInvoice);
            Context.Entry(CommisionInvoice).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete CommisionInvoice with specific id
        /// </summary>
        /// <param name="Id">CommisionInvoice Id</param>
        public void DeleteCommisionInvoice(int Id)
        {
            CommisionInvoice deletedCommisionInvoice = GetCommisionInvoiceById(Id);
            Context.CommisionInvoice.Remove(deletedCommisionInvoice);
            Context.Entry(deletedCommisionInvoice).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a CommisionInvoice with its Id and Update IsDeleted IF that CommisionInvoice has relationship with others
        /// </summary>
        /// <param name="CommisionInvoiceId">Id of CommisionInvoice</param>
        public void DeleteCommisionInvoiceRs(int Id)
        {
            CommisionInvoice deleteCommisionInvoiceRs = GetCommisionInvoiceById(Id);
            deleteCommisionInvoiceRs.IsDeleted = true;
            UpdateCommisionInvoice(deleteCommisionInvoiceRs);
        }

        /// <summary>
        /// Update CommisionInvoice into database
        /// </summary>
        /// <param name="CommisionInvoice">CommisionInvoice object</param>
        public void UpdateCommisionInvoice(CommisionInvoice CommisionInvoice)
        {
            Context.Entry(CommisionInvoice).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
