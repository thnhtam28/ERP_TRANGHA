using Erp.Domain.Crm.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Erp.Domain.Crm.Entities;

namespace Erp.Domain.Crm.Repositories
{
    public class CRM_KH_BANHANGRepository : GenericRepository<ErpCrmDbContext, CRM_KH_BANHANG>, ICRM_KH_BANHANGRepository
    {
        public CRM_KH_BANHANGRepository(ErpCrmDbContext context) : base(context)
        {
        }

      
        /// <summary>
        /// Get all AdviseCard
        /// </summary>
        /// <returns>AdviseCard list</returns>
        public IQueryable<CRM_KH_BANHANG> GetAllCRM_KH_BANHANG()
        {
            return Context.CRM_KH_BANHANG.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public  IQueryable<vwCRM_KH_BANHANG> GetAllvwCRM_KH_BANHANG()
        {
            return Context.vwCRM_KH_BANHANG.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public List<vwCRM_KH_BANHANG> GetListvwCRM_KH_BANHANG()
        {
            return Context.vwCRM_KH_BANHANG.Where(item => (item.IsDeleted == null || item.IsDeleted == false)).ToList();
        }
        public int InsertProductInvoice(CRM_Sale_ProductInvoice ProductInvoice, List<CRM_Sale_ProductInvoiceDetail> orderDetails)
        {
            Context.CRM_Sale_ProductInvoice.Add(ProductInvoice);
            Context.Entry(ProductInvoice).State = EntityState.Added;
            Context.SaveChanges();
            if (orderDetails != null)
            {
                for (int i = 0; i < orderDetails.Count; i++)
                {
                    orderDetails[i].ProductInvoiceId = ProductInvoice.Id;
                    InsertProductInvoiceDetail(orderDetails[i]);
                }
            }
            return ProductInvoice.Id;
        }
        public void InsertProductInvoiceDetail(CRM_Sale_ProductInvoiceDetail ProductInvoiceDetail)
        {
            Context.CRM_Sale_ProductInvoiceDetail.Add(ProductInvoiceDetail);
            Context.Entry(ProductInvoiceDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        public IQueryable<vwCRM_Sale_ProductInvoice> GetAllvwCRM_Sale_ProductInvoice()
        {
            return Context.vwCRM_Sale_ProductInvoice.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        
        public CRM_Sale_ProductInvoice GetCRM_Sale_ProductInvoiceById(int Id)
        {
            return Context.CRM_Sale_ProductInvoice.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public CRM_KH_BANHANG_CTIET GetCRM_KH_BANHANG_CTById(int KH_BANHANG_CTIET_ID)
        {
            return Context.CRM_KH_BANHANG_CTIET.SingleOrDefault(item => item.KH_BANHANG_CTIET_ID == KH_BANHANG_CTIET_ID && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public CRM_KH_BANHANG GetCRM_KH_BANHANGById(int Id)
        {
            return Context.CRM_KH_BANHANG.SingleOrDefault(item => item.KH_BANHANG_ID == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public CRM_KH_BANHANG GetCRM_KH_BANHANGByUserId(int Id)
        {
            return Context.CRM_KH_BANHANG.SingleOrDefault(item => item.NGUOILAP_ID == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwCRM_KH_BANHANG GetvwCRM_KH_BANHANGById(int Id)
        {
            return Context.vwCRM_KH_BANHANG.SingleOrDefault(item => item.KH_BANHANG_ID == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert AdviseCard into database
        /// </summary>
        /// <param name="AdviseCard">Object infomation</param>
        public void InsertCRM_KH_BANHANG(CRM_KH_BANHANG CRM_KH_BANHANG)
        {
            Context.CRM_KH_BANHANG.Add(CRM_KH_BANHANG);
            Context.Entry(CRM_KH_BANHANG).State = EntityState.Added;
            Context.SaveChanges();
        }

      
        public void DeleteCRM_KH_BANHANG(int Id)
        {
            CRM_KH_BANHANG deletedCRM_KH_BANHANG = GetCRM_KH_BANHANGById(Id);
            Context.CRM_KH_BANHANG.Remove(deletedCRM_KH_BANHANG);
            Context.Entry(deletedCRM_KH_BANHANG).State = EntityState.Deleted;
            Context.SaveChanges();
        }

      
        public void DeleteCRM_KH_BANHANGRs(int Id)
        {
            CRM_KH_BANHANG deleteCRM_KH_BANHANGRs = GetCRM_KH_BANHANGById(Id);
            deleteCRM_KH_BANHANGRs.IsDeleted = true;
            UpdateCRM_KH_BANHANG(deleteCRM_KH_BANHANGRs);
        }
        
        public void UpdateCRM_KH_BANHANG_CT(CRM_KH_BANHANG_CTIET CRM_KH_BANHANG_CT)
        {
            Context.Entry(CRM_KH_BANHANG_CT).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void UpdateCRMProductInvoice(CRM_Sale_ProductInvoice CRM_Sale_ProductInvoice)
        {
            Context.Entry(CRM_Sale_ProductInvoice).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void UpdateCRM_KH_BANHANG(CRM_KH_BANHANG CRM_KH_BANHANG)
        {
            Context.Entry(CRM_KH_BANHANG).State = EntityState.Modified;
            Context.SaveChanges();
        }
        
        public IQueryable<vwCRM_KH_BANHANG_CTIET> GetAllvwCRM_KH_BANHANG_CTIET()
        {
            return Context.vwCRM_KH_BANHANG_CTIET.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        
        public IQueryable<vwKH_SAPHETSP> GetAllvwKH_SAPHETSP()
        {
            return Context.vwKH_SAPHETSP;
        }
        public IQueryable<vwCustomer_ProductInvoice> GetAllvwCustomer_ProductInvoice()
        {
            return Context.vwCustomer_ProductInvoice;
        }

        public vwCRM_Sale_ProductInvoice GetvwCRM_Sale_ProductInvoiceById(int Id)
        {
            return Context.vwCRM_Sale_ProductInvoice.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwCRM_Sale_ProductInvoice GetvwCRM_Sale_ProductInvoiceFullById(int Id)
        {
            return Context.vwCRM_Sale_ProductInvoice.SingleOrDefault(item => item.Id == Id);
        }
        public vwCRM_Sale_ProductInvoice GetvwCRM_Sale_ProductInvoiceByCode(string code)
        {
            return Context.vwCRM_Sale_ProductInvoice.SingleOrDefault(item => item.Code == code && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwCRM_Sale_ProductInvoiceDetail> GetAllvwInvoiceDetailsByInvoiceId(int InvoiceId)
        {
            return Context.vwCRM_Sale_ProductInvoiceDetail.Where(item => item.ProductInvoiceId == InvoiceId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<CRM_Sale_ProductInvoiceDetail> GetAllInvoiceDetailsByInvoiceId(int InvoiceId)
        {
            return Context.CRM_Sale_ProductInvoiceDetail.Where(item => item.ProductInvoiceId == InvoiceId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void DeleteDetailsInvoice(int Id)
        {
            CRM_Sale_ProductInvoiceDetail deletedCRM_KH_BANHANG = GetSale_ProductInvoiceDetailById(Id);
            Context.CRM_Sale_ProductInvoiceDetail.Remove(deletedCRM_KH_BANHANG);
            Context.Entry(deletedCRM_KH_BANHANG).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public CRM_Sale_ProductInvoiceDetail GetSale_ProductInvoiceDetailById(int Id)
        {
            return Context.CRM_Sale_ProductInvoiceDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        
    }
}
