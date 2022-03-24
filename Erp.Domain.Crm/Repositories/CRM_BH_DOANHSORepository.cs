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
    public class CRM_BH_DOANHSORepository : GenericRepository<ErpCrmDbContext, CRM_BH_DOANHSO>, ICRM_BH_DOANHSORepository
    {
        public CRM_BH_DOANHSORepository(ErpCrmDbContext context)
           : base(context)
        {


        }
        public void UpdateCRM_BH_DOANHSO_CT(CRM_BH_DOANHSO_CT CRM_BH_DOANHSO_CT)
        {
            Context.Entry(CRM_BH_DOANHSO_CT).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public CRM_BH_DOANHSO_CT GetCRM_BH_DOANHSO_CTById(int BranchId, int Id)
        {
            return Context.CRM_BH_DOANHSO_CT.SingleOrDefault(item => item.BranchId== BranchId && item.KH_BANHANG_DOANHSO_CTIET_ID == Id && (item.IsDeleted == false));
        }
        public CRM_BH_DOANHSO GetCRM_BH_DOANHSOById(int Id)
        {
            return Context.CRM_BH_DOANHSO.SingleOrDefault(item => item.KH_BANHANG_DOANHSO_ID == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertCRM_BH_DOANHSO_CT(CRM_BH_DOANHSO_CT CRM_BH_DOANHSO_CT)
        {
            Context.CRM_BH_DOANHSO_CT.Add(CRM_BH_DOANHSO_CT);
            Context.Entry(CRM_BH_DOANHSO_CT).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void DeleteCRM_BH_DOANHSO_CTRs(int Id, int BranchId)
        {
            CRM_BH_DOANHSO_CT deleteCRM_KH_BANHANGRs = GetCRM_BH_DOANHSO_CTById(BranchId,Id);
            deleteCRM_KH_BANHANGRs.IsDeleted = true;
            UpdateCRM_BH_DOANHSO_CT(deleteCRM_KH_BANHANGRs);
        }

        public IQueryable<vwCRM_BH_DOANHSO_CT> GetAllvwCRM_BH_DOANHSO_CT()
        {
            return Context.vwCRM_BH_DOANHSO_CT.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<CRM_BH_DOANHSO> GetAllCRM_BH_DOANHSO()
        {
            return Context.CRM_BH_DOANHSO.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwCRM_BH_DOANHSO> GetAllvwCRM_BH_DOANHSO()
        {
            return Context.vwCRM_BH_DOANHSO.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public List<vwCRM_BH_DOANHSO> GetlistAllvwCRM_BH_DOANHSO()
        {
            return Context.vwCRM_BH_DOANHSO.Where(item => (item.IsDeleted == null || item.IsDeleted == false)).ToList();
        }


        public void InserCRM_BH_DOANHSO(CRM_BH_DOANHSO CRM_BH_DOANHSO)
        {
            Context.CRM_BH_DOANHSO.Add(CRM_BH_DOANHSO);
            Context.Entry(CRM_BH_DOANHSO).State = EntityState.Added;
            Context.SaveChanges();
        }
        public void UpdateCRM_BH_DOANHSO(CRM_BH_DOANHSO CRM_BH_DOANHSO)
        {
            Context.Entry(CRM_BH_DOANHSO).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void DeleteCRM_BH_DOANHSORs(int Id)
        {
            CRM_BH_DOANHSO deleteCRM_KH_BANHANGRs = GetCRM_BH_DOANHSOById(Id);
            deleteCRM_KH_BANHANGRs.IsDeleted = true;
            UpdateCRM_BH_DOANHSO(deleteCRM_KH_BANHANGRs);
        }


    }
}
