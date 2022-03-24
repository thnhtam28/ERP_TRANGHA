using System.Collections.Generic;
using System.Linq;
using Erp.Domain.Crm.Entities;

namespace Erp.Domain.Crm.Interfaces
{
    public interface ICRM_KH_BANHANGRepository
    {
        
        IQueryable<CRM_KH_BANHANG> GetAllCRM_KH_BANHANG();
        //IQueryable<vwAdviseCard> GetvwAllAdviseCard();
        IQueryable<vwCRM_KH_BANHANG> GetAllvwCRM_KH_BANHANG();

        List<vwCRM_KH_BANHANG> GetListvwCRM_KH_BANHANG();

        IQueryable<vwCRM_Sale_ProductInvoice> GetAllvwCRM_Sale_ProductInvoice();
        vwCRM_KH_BANHANG GetvwCRM_KH_BANHANGById(int Id);

        vwCRM_Sale_ProductInvoice GetvwCRM_Sale_ProductInvoiceById(int Id);
        vwCRM_Sale_ProductInvoice GetvwCRM_Sale_ProductInvoiceFullById(int Id);
        vwCRM_Sale_ProductInvoice GetvwCRM_Sale_ProductInvoiceByCode(string code);

        CRM_KH_BANHANG GetCRM_KH_BANHANGByUserId(int Id);
        CRM_KH_BANHANG GetCRM_KH_BANHANGById(int Id);
        CRM_KH_BANHANG_CTIET GetCRM_KH_BANHANG_CTById(int KH_BANHANG_CTIET_ID);
        CRM_Sale_ProductInvoice GetCRM_Sale_ProductInvoiceById(int Id);
        //vwAdviseCard GetvwAdviseCardById(int Id);
        /// <summary>
        /// Insert AdviseCard into database
        /// </summary>
        /// <param name="AdviseCard">Object infomation</param>
        void InsertCRM_KH_BANHANG(CRM_KH_BANHANG CRM_KH_BANHANG);
        int InsertProductInvoice(CRM_Sale_ProductInvoice ProductInvoice, List<CRM_Sale_ProductInvoiceDetail> orderDetails);
        void InsertProductInvoiceDetail(CRM_Sale_ProductInvoiceDetail ProductInvoiceDetail);
        /// <summary>
        /// Delete AdviseCard with specific id
        /// </summary>
        /// <param name="Id">AdviseCard Id</param>
        void DeleteCRM_KH_BANHANG(int Id);

        /// <summary>
        /// Delete a AdviseCard with its Id and Update IsDeleted IF that AdviseCard has relationship with others
        /// </summary>
        /// <param name="Id">Id of AdviseCard</param>
        void DeleteCRM_KH_BANHANGRs(int Id);

        /// <summary>
        /// Update AdviseCard into database
        /// </summary>
        /// <param name="AdviseCard">AdviseCard object</param>
        void UpdateCRM_KH_BANHANG(CRM_KH_BANHANG CRM_KH_BANHANG);
        void UpdateCRM_KH_BANHANG_CT(CRM_KH_BANHANG_CTIET CRM_KH_BANHANG_CT);
        void UpdateCRMProductInvoice(CRM_Sale_ProductInvoice CRM_Sale_ProductInvoice);
        IQueryable<vwCustomer_ProductInvoice> GetAllvwCustomer_ProductInvoice();
        IQueryable<vwCRM_KH_BANHANG_CTIET> GetAllvwCRM_KH_BANHANG_CTIET();

        IQueryable<vwKH_SAPHETSP> GetAllvwKH_SAPHETSP();

        IQueryable<vwCRM_Sale_ProductInvoiceDetail> GetAllvwInvoiceDetailsByInvoiceId(int InvoiceId);

        IQueryable<CRM_Sale_ProductInvoiceDetail> GetAllInvoiceDetailsByInvoiceId(int InvoiceId);

        void DeleteDetailsInvoice(int Id);
    }
}
