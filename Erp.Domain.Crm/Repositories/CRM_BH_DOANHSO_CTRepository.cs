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
    public class CRM_BH_DOANHSO_CTRepository : GenericRepository<ErpCrmDbContext, CRM_BH_DOANHSO_CT>, ICRM_BH_DOANHSO_CTRepository
    {
        public CRM_BH_DOANHSO_CTRepository(ErpCrmDbContext context)
           : base(context)
        {


        }
        public void InsertCRM_BH_DOANHSO_CTIET(CRM_BH_DOANHSO_CT CRM_BH_DOANHSO_CTIET)
        {
            Context.CRM_BH_DOANHSO_CT.Add(CRM_BH_DOANHSO_CTIET);
            Context.Entry(CRM_BH_DOANHSO_CTIET).State = EntityState.Added;
            Context.SaveChanges();
        }
        //public IQueryable<CRM_BH_DOANHSO_CT> GetAllCRM_BH_CT()
        //{
        //    return Context.CRM_BH_DOANHSO_CT;
        //}
        //public IQueryable<vwCRM_BH_DOANHSO_CT> GetAllvwCRM_BH_CT()
        //{
        //    return Context.vwCRM_BH_DOANHSO_CT.Where(x => x.IsDeleted == null || x.IsDeleted == false);
        //}

    }
}
