using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class DonateProOrSerRepository : GenericRepository<ErpSaleDbContext, DonateProOrSer>, IDonateProOrSerRepository
    {
        public DonateProOrSerRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all DonateProOrSer
        /// </summary>
        /// <returns>DonateProOrSer list</returns>
        public IQueryable<DonateProOrSer> GetAllDonateProOrSer()
        {
            return Context.DonateProOrSer.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwDonateProOrSer> GetvwAllDonateProOrSer()
        {
            return Context.vwDonateProOrSer.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get DonateProOrSer information by specific id
        /// </summary>
        /// <param name="DonateProOrSerId">Id of DonateProOrSer</param>
        /// <returns></returns>
        public DonateProOrSer GetDonateProOrSerById(int Id)
        {
            return Context.DonateProOrSer.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwDonateProOrSer GetvwDonateProOrSerById(int Id)
        {
            return Context.vwDonateProOrSer.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert DonateProOrSer into database
        /// </summary>
        /// <param name="DonateProOrSer">Object infomation</param>
        public void InsertDonateProOrSer(DonateProOrSer DonateProOrSer)
        {
            Context.DonateProOrSer.Add(DonateProOrSer);
            Context.Entry(DonateProOrSer).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete DonateProOrSer with specific id
        /// </summary>
        /// <param name="Id">DonateProOrSer Id</param>
        public void DeleteDonateProOrSer(int Id)
        {
            DonateProOrSer deletedDonateProOrSer = GetDonateProOrSerById(Id);
            Context.DonateProOrSer.Remove(deletedDonateProOrSer);
            Context.Entry(deletedDonateProOrSer).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a DonateProOrSer with its Id and Update IsDeleted IF that DonateProOrSer has relationship with others
        /// </summary>
        /// <param name="DonateProOrSerId">Id of DonateProOrSer</param>
        public void DeleteDonateProOrSerRs(int Id)
        {
            DonateProOrSer deleteDonateProOrSerRs = GetDonateProOrSerById(Id);
            deleteDonateProOrSerRs.IsDeleted = true;
            UpdateDonateProOrSer(deleteDonateProOrSerRs);
        }

        /// <summary>
        /// Update DonateProOrSer into database
        /// </summary>
        /// <param name="DonateProOrSer">DonateProOrSer object</param>
        public void UpdateDonateProOrSer(DonateProOrSer DonateProOrSer)
        {
            Context.Entry(DonateProOrSer).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
