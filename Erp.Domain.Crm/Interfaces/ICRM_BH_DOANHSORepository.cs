using Erp.Domain.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Interfaces
{
    public interface ICRM_BH_DOANHSORepository
    {
        IQueryable<CRM_BH_DOANHSO> GetAllCRM_BH_DOANHSO();
        IQueryable<vwCRM_BH_DOANHSO> GetAllvwCRM_BH_DOANHSO();
        List<vwCRM_BH_DOANHSO> GetlistAllvwCRM_BH_DOANHSO();
        //IQueryable<vwCRM_BH_DOANHSO_CT> GetAllvwCRM_BH_DOANHSO_CT();
        ////void InsertCRM_BH_DOANHSO(CRM_BH_DOANHSO CRM_BH_DOANHSO);
        void InserCRM_BH_DOANHSO(CRM_BH_DOANHSO CRM_BH_DOANHSO);
        void InsertCRM_BH_DOANHSO_CT(CRM_BH_DOANHSO_CT CRM_BH_DOANHSO_CT);
        /// <summary>
        /// Get Answer information by specific id
        /// </summary>
        /// <param name="Id">Id of Answer</param>
        /// <returns></returns>
        /// 
        void UpdateCRM_BH_DOANHSO_CT(CRM_BH_DOANHSO_CT CRM_BH_DOANHSO_CT);
        void UpdateCRM_BH_DOANHSO(CRM_BH_DOANHSO CRM_BH_DOANHSO);
        IQueryable<vwCRM_BH_DOANHSO_CT> GetAllvwCRM_BH_DOANHSO_CT();
        CRM_BH_DOANHSO GetCRM_BH_DOANHSOById(int Id);
        CRM_BH_DOANHSO_CT GetCRM_BH_DOANHSO_CTById(int BranchId, int Id);
        void DeleteCRM_BH_DOANHSO_CTRs(int Id, int BranchId);
        void DeleteCRM_BH_DOANHSORs(int Id);
    }
}
