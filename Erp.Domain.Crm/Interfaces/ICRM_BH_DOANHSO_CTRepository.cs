using Erp.Domain.Crm.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Crm.Interfaces
{
    public interface ICRM_BH_DOANHSO_CTRepository
    {
        //IQueryable<CRM_BH_DOANHSO_CT> GetAllCRM_BH_CT();
        //IQueryable<vwCRM_BH_DOANHSO_CT> GetAllvwCRM_BH_CT();

        void InsertCRM_BH_DOANHSO_CTIET(CRM_BH_DOANHSO_CT CRM_BH_DOANHSO_CTIET);
        /// <summary>
        /// Get Answer information by specific id
        /// </summary>
        /// <param name="Id">Id of Answer</param>
        /// <returns></returns>
   
    }
}
