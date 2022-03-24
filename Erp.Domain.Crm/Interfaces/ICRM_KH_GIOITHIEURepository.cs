using Erp.Domain.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Interfaces
{
    public interface ICRM_KH_GIOITHIEURepository
    {
        IQueryable<CRM_KH_GIOITHIEU> GetAllCRM_KH_GIOITHIEU();
        IQueryable<vwKH_GIOITHIEU> GetAllvwKH_GIOITHIEU();
        /// <summary>
        /// Get Answer information by specific id
        /// </summary>
        /// <param name="Id">Id of Answer</param>
        /// <returns></returns>
        CRM_KH_GIOITHIEU GetCRM_KH_GIOITHIEUById(int Id);
        vwKH_GIOITHIEU GetvwKH_GIOITHIEUById(int Id);
        /// <summary>
        /// Insert Answer into database
        /// </summary>
        /// <param name="Answer">Object infomation</param>
        void InsertCRM_KH_GIOITHIEU(CRM_KH_GIOITHIEU CRM_KH_GIOITHIEU);

        /// <summary>
        /// Delete Answer with specific id
        /// </summary>
        /// <param name="Id">Answer Id</param>
        void DeleteCRM_KH_GIOITHIEU(int Id);

        /// <summary>
        /// Delete a Answer with its Id and Update IsDeleted IF that Answer has relationship with others
        /// </summary>
        /// <param name="Id">Id of Answer</param>
        void DeleteCRM_KH_GIOITHIEURs(int Id);

        /// <summary>
        /// Update Answer into database
        /// </summary>
        /// <param name="Answer">Answer object</param>
        void UpdateCRM_KH_GIOITHIEU(CRM_KH_GIOITHIEU CRM_KH_GIOITHIEU);
    }
}
