using Erp.Domain.Crm.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Erp.Domain.Crm.Entities;

namespace Erp.Domain.Crm.Repositories
{
    public class CRM_KH_BANHANG_CTIETRepository : GenericRepository<ErpCrmDbContext, CRM_KH_BANHANG_CTIET>, ICRM_KH_BANHANG_CTIETRepository
    {
        public CRM_KH_BANHANG_CTIETRepository(ErpCrmDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get all AdviseCard
        /// </summary>
        /// <returns>AdviseCard list</returns>
        public IQueryable<CRM_KH_BANHANG_CTIET> GetAllCRM_KH_BANHANG_CTIET()
        {
            return Context.CRM_KH_BANHANG_CTIET.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
 

        public CRM_KH_BANHANG_CTIET GetCRM_KH_BANHANG_CTIETById(int Id)
        {
            return Context.CRM_KH_BANHANG_CTIET.SingleOrDefault(item => item.KH_BANHANG_CTIET_ID == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
    /// <summary>
    /// Insert AdviseCard into database
    /// </summary>
    /// <param name="AdviseCard">Object infomation</param>
        public void  InsertCRM_KH_BANHANG_CTIET(CRM_KH_BANHANG_CTIET CRM_KH_BANHANG_CTIET)
        {
            Context.CRM_KH_BANHANG_CTIET.Add(CRM_KH_BANHANG_CTIET);
            Context.Entry(CRM_KH_BANHANG_CTIET).State = EntityState.Added;
            Context.SaveChanges();
        }


        public void DeleteCRM_KH_BANHANG_CTIET(int Id)
        {
            CRM_KH_BANHANG_CTIET deletedCRM_KH_BANHANG_CTIET = GetCRM_KH_BANHANG_CTIETById(Id);
            Context.CRM_KH_BANHANG_CTIET.Remove(deletedCRM_KH_BANHANG_CTIET);
            Context.Entry(deletedCRM_KH_BANHANG_CTIET).State = EntityState.Deleted;
            Context.SaveChanges();
        }


        public void DeleteCRM_KH_BANHANG_CTIETRs(int Id)
        {
            CRM_KH_BANHANG_CTIET deleteCRM_KH_BANHANGRs = GetCRM_KH_BANHANG_CTIETById(Id);
            deleteCRM_KH_BANHANGRs.IsDeleted = true;
            UpdateCRM_KH_BANHANG_CTIET(deleteCRM_KH_BANHANGRs);
        }


        public void UpdateCRM_KH_BANHANG_CTIET(CRM_KH_BANHANG_CTIET CRM_KH_BANHANG_CTIET)
        {
            Context.Entry(CRM_KH_BANHANG_CTIET).State = EntityState.Modified;
            Context.SaveChanges();
        }

         public IQueryable<CRM_KH_BANHANG_CTIET> GetAllCRM_KH_BANHANG_CTIETByCRM_KH_BANHANGId(int InvoiceId)
        {
            return Context.CRM_KH_BANHANG_CTIET.Where(item => item.KH_BANHANG_ID == InvoiceId && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwCRM_KH_BANHANG_CTIET> GetAllvwCRM_KH_BANHANG_CTIET()
        {
            return Context.vwCRM_KH_BANHANG_CTIET.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwCRM_KH_BANHANG_CTIET GetvwCRM_KH_BANHANG_CTIETById(int KH_TUONGTAC_ID)
        {
            return Context.vwCRM_KH_BANHANG_CTIET.SingleOrDefault(item => item.KH_BANHANG_CTIET_ID == KH_TUONGTAC_ID && (item.IsDeleted == null || item.IsDeleted == false));
        }
    }
}
