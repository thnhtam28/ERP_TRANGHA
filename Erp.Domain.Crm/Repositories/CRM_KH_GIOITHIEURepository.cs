using Erp.Domain.Crm.Entities;
using Erp.Domain.Crm.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Repositories
{
    public class CRM_KH_GIOITHIEURepository : GenericRepository<ErpCrmDbContext, CRM_KH_GIOITHIEU>, ICRM_KH_GIOITHIEURepository
    {
        public CRM_KH_GIOITHIEURepository(ErpCrmDbContext context)
           : base(context)
        {

        }

        /// <summary>
        /// Get all Answer
        /// </summary>
        /// <returns>Answer list</returns>
        public IQueryable<CRM_KH_GIOITHIEU> GetAllCRM_KH_GIOITHIEU()
        {
            return Context.CRM_KH_GIOITHIEU.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwKH_GIOITHIEU> GetAllvwKH_GIOITHIEU()
        {
            return Context.vwKH_GIOITHIEU.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get Answer information by specific id
        /// </summary>
        /// <param name="AnswerId">Id of Answer</param>
        /// <returns></returns>
        public CRM_KH_GIOITHIEU GetCRM_KH_GIOITHIEUById(int KH_GIOITHIEU_ID)
        {
            return Context.CRM_KH_GIOITHIEU.SingleOrDefault(item => item.KH_GIOITHIEU_ID == KH_GIOITHIEU_ID && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwKH_GIOITHIEU GetvwKH_GIOITHIEUById(int Id)
        {
            return Context.vwKH_GIOITHIEU.SingleOrDefault(item => item.KH_GIOITHIEU_ID == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert Answer into database
        /// </summary>
        /// <param name="Answer">Object infomation</param>
        public void InsertCRM_KH_GIOITHIEU(CRM_KH_GIOITHIEU CRM_KH_GIOITHIEU)
        {
            Context.CRM_KH_GIOITHIEU.Add(CRM_KH_GIOITHIEU);
            Context.Entry(CRM_KH_GIOITHIEU).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete Answer with specific id
        /// </summary>
        /// <param name="Id">Answer Id</param>
        public void DeleteCRM_KH_GIOITHIEU(int KH_GIOITHIEU_ID)
        {
            CRM_KH_GIOITHIEU deletedCRM_KH_GIOITHIEU = GetCRM_KH_GIOITHIEUById(KH_GIOITHIEU_ID);
            Context.CRM_KH_GIOITHIEU.Remove(deletedCRM_KH_GIOITHIEU);
            Context.Entry(deletedCRM_KH_GIOITHIEU).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete a Answer with its Id and Update IsDeleted IF that Answer has relationship with others
        /// </summary>
        /// <param name="CRM_KH_GIOITHIEUId">Id of Answer</param>
        public void DeleteCRM_KH_GIOITHIEURs(int Id)
        {
            CRM_KH_GIOITHIEU deleteCRM_KH_GIOITHIEURs = GetCRM_KH_GIOITHIEUById(Id);
            deleteCRM_KH_GIOITHIEURs.IsDeleted = true;
            UpdateCRM_KH_GIOITHIEU(deleteCRM_KH_GIOITHIEURs);
        }

        /// <summary>
        /// Update Answer into database
        /// </summary>
        /// <param name="CRM_KH_GIOITHIEU">Answer object</param>
        public void UpdateCRM_KH_GIOITHIEU(CRM_KH_GIOITHIEU CRM_KH_GIOITHIEU)
        {
            Context.Entry(CRM_KH_GIOITHIEU).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
