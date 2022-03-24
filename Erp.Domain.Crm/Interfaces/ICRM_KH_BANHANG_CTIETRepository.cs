using Erp.Domain.Crm.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Crm.Interfaces
{
    public interface ICRM_KH_BANHANG_CTIETRepository
    {

        IQueryable<CRM_KH_BANHANG_CTIET> GetAllCRM_KH_BANHANG_CTIET();
        //IQueryable<vwAdviseCard> GetvwAllAdviseCard();




        CRM_KH_BANHANG_CTIET GetCRM_KH_BANHANG_CTIETById(int Id);
        //vwAdviseCard GetvwAdviseCardById(int Id);
        /// <summary>
        /// Insert AdviseCard into database
        /// </summary>
        /// <param name="AdviseCard">Object infomation</param>
        void InsertCRM_KH_BANHANG_CTIET(CRM_KH_BANHANG_CTIET CRM_KH_BANHANG_CTIET);

        /// <summary>
        /// Delete AdviseCard with specific id
        /// </summary>
        /// <param name="Id">AdviseCard Id</param>
        void DeleteCRM_KH_BANHANG_CTIET(int Id);

        /// <summary>
        /// Delete a AdviseCard with its Id and Update IsDeleted IF that AdviseCard has relationship with others
        /// </summary>
        /// <param name="Id">Id of AdviseCard</param>
        void DeleteCRM_KH_BANHANG_CTIETRs(int Id);

        /// <summary>
        /// Update AdviseCard into database
        /// </summary>
        /// <param name="AdviseCard">AdviseCard object</param>
        void UpdateCRM_KH_BANHANG_CTIET(CRM_KH_BANHANG_CTIET CRM_KH_BANHANG_CTIET);
        IQueryable<CRM_KH_BANHANG_CTIET> GetAllCRM_KH_BANHANG_CTIETByCRM_KH_BANHANGId(int InvoiceId);
        IQueryable<vwCRM_KH_BANHANG_CTIET> GetAllvwCRM_KH_BANHANG_CTIET();

        vwCRM_KH_BANHANG_CTIET GetvwCRM_KH_BANHANG_CTIETById(int KH_TUONGTAC_ID);


    }
}
