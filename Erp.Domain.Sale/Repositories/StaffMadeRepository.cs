using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class StaffMadeRepository : GenericRepository<ErpSaleDbContext, StaffMade>, IStaffMadeRepository
    {
        public StaffMadeRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all StaffMade
        /// </summary>
        /// <returns>StaffMade list</returns>
        public IQueryable<StaffMade> GetAllStaffMade()
        {
            return Context.StaffMade.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwStaffMade> GetvwAllStaffMade()
        {
            return Context.vwStaffMade.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get StaffMade information by specific id
        /// </summary>
        /// <param name="StaffMadeId">Id of StaffMade</param>
        /// <returns></returns>
        public StaffMade GetStaffMadeById(int Id)
        {
            return Context.StaffMade.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwStaffMade GetvwStaffMadeById(int Id)
        {
            return Context.vwStaffMade.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert StaffMade into database
        /// </summary>
        /// <param name="StaffMade">Object infomation</param>
        public void InsertStaffMade(StaffMade StaffMade)
        {
            Context.StaffMade.Add(StaffMade);
            Context.Entry(StaffMade).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete StaffMade with specific id
        /// </summary>
        /// <param name="Id">StaffMade Id</param>
        public void DeleteStaffMade(int Id)
        {
            StaffMade deletedStaffMade = GetStaffMadeById(Id);
            Context.StaffMade.Remove(deletedStaffMade);
            Context.Entry(deletedStaffMade).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a StaffMade with its Id and Update IsDeleted IF that StaffMade has relationship with others
        /// </summary>
        /// <param name="StaffMadeId">Id of StaffMade</param>
        public void DeleteStaffMadeRs(int Id)
        {
            StaffMade deleteStaffMadeRs = GetStaffMadeById(Id);
            deleteStaffMadeRs.IsDeleted = true;
            UpdateStaffMade(deleteStaffMadeRs);
        }

        /// <summary>
        /// Update StaffMade into database
        /// </summary>
        /// <param name="StaffMade">StaffMade object</param>
        public void UpdateStaffMade(StaffMade StaffMade)
        {
            Context.Entry(StaffMade).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
