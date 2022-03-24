using Erp.Domain.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Interfaces
{
    public interface IKH_TUONGTACRepository
    {
        IQueryable<KH_TUONGTAC> GetAllKH_TUONGTAC();
        IQueryable<vwCRM_KH_TUONGTAC> GetAllvwKH_TUONGTAC();
        IQueryable<KH_TUONGTAC> GetAllKH_TUONGTACById(int KH_TUONGTAC_ID);
        vwCRM_KH_TUONGTAC GetvwKH_TUONGTACById(int KH_TUONGTAC_ID);
        IQueryable<vwCRM_TK_TUONGTAC> GetAllvwCRM_TK_TUONGTAC();
        /// <summary>
        /// Get Answer information by specific id
        /// </summary>
        /// <param name="KH_TUONGTAC_ID">Id of Answer</param>
        /// <returns></returns>
        KH_TUONGTAC GetKH_TUONGTACById(int? KH_TUONGTAC_ID);
        //vwKH_GIOITHIEU GetvwKH_GIOITHIEUById(int Id);
        /// <summary>
        /// Insert Answer into database
        /// </summary>
        /// <param name="KH_TUONGTAC">Object infomation</param>
        void InsertKH_TUONGTAC(KH_TUONGTAC CRMKH_TUONGTAC);

        /// <summary>
        /// Delete Answer with specific id
        /// </summary>
        /// <param name="KH_TUONGTAC_ID">Answer Id</param>
        void DeleteKH_TUONGTAC(int KH_TUONGTAC_ID);

        /// <summary>
        /// Delete a Answer with its Id and Update IsDeleted IF that Answer has relationship with others
        /// </summary>
        /// <param name="KH_TUONGTAC_ID">Id of Answer</param>
        void DeleteKH_TUONGTACRs(int KH_TUONGTAC_ID);

        /// <summary>
        /// Update Answer into database
        /// </summary>
        /// <param name="KH_TUONGTAC">Answer object</param>
        void UpdateKH_TUONGTAC(KH_TUONGTAC KH_TUONGTAC);
        IQueryable<vwCRM_KH_TUONGTAC> GetvwKH_TUONGTACByALL();
    }
}
