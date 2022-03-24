using Erp.Domain.Sale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Erp.Domain.Sale.Interfaces
{
    public interface IProductInvoiceRepository
    {
        /// <summary>
        /// Get all ProductInvoice
        /// </summary>
        /// <returns>ProductInvoice list</returns>
        IQueryable<ProductInvoice> GetAllProductInvoice();
        IQueryable<vwProductInvoice> GetAllvwProductInvoice();
        IQueryable<vwProductInvoice> GetAllvwProductInvoiceFull();
        List<vwProductInvoice> GetAllvwProductInvoiceFulls();
        IQueryable<vwProductInvoice_return> GetAllvwProductInvoiceFull_return();
        IQueryable<vwProductInvoice_NVKD> GetAllvwProductInvoiceFull_NVKD();

        /// <summary>
        /// Get ProductInvoice information by specific id
        /// </summary>
        /// <param name="Id">Id of ProductInvoice</param>
        /// <returns></returns>
        ProductInvoice GetProductInvoiceById(int? Id);
        vwProductInvoice GetvwProductInvoiceById(int Id);
        vwProductInvoice GetvwProductInvoiceFullById(int Id);
        vwProductInvoice GetvwProductInvoiceByCode(int pBranchId,string code);
        vwProductInvoice_NVKD GetvwProductInvoice_NVKDById(int Id);
        /// <summary>
        /// Insert ProductInvoice into database
        /// </summary>
        /// <param name="ProductInvoice">Object infomation</param>
        int InsertProductInvoice(ProductInvoice ProductInvoice, List<ProductInvoiceDetail> orderDetails);
        void InsertMoneyMove(MoneyMove MoneyMove);
        MoneyMove GetMMByOldId(int old);
        MoneyMove GetMMByNewId(int old);
        MoneyMove GetMMById(int old);
        void DeleteMoneyMove(int Id);
        void UpdateMoneyMove(MoneyMove ProductInvoice);
        /// <summary>
        /// Delete ProductInvoice with specific id
        /// </summary>
        /// <param name="Id">ProductInvoice Id</param>
        void DeleteProductInvoice(int Id);

        /// <summary>
        /// Delete a ProductInvoice with its Id and Update IsDeleted IF that ProductInvoice has relationship with others
        /// </summary>
        /// <param name="Id">Id of ProductInvoice</param>
        void DeleteProductInvoiceRs(int Id);

        void UpdateProductInvoice(ProductInvoice ProductInvoice);


        // Order detail
        IQueryable<vwProductInvoiceDetail> GetAllvwInvoiceDetails();
        IQueryable<ProductInvoiceDetail> GetAllInvoiceDetailsByInvoiceId(int InvoiceId);
        List<ProductInvoiceDetail> GetListAllInvoiceDetailsByInvoiceId(int InvoiceId);
        IQueryable<vwProductInvoiceDetail> GetAllvwInvoiceDetailsByInvoiceId(int InvoiceId);
        ProductInvoiceDetail GetProductInvoiceDetailById(int Id);
        vwProductInvoiceDetail GetvwProductInvoiceDetailById(Int64 Id);
    
        void InsertProductInvoiceDetail(ProductInvoiceDetail ProductInvoiceDetail);

     
        void DeleteProductInvoiceDetail(int Id);
        void DeleteProductInvoiceDetail(List<ProductInvoiceDetail> list);
        
        void DeleteProductInvoiceDetailRs(int Id);

        
        void UpdateProductInvoiceDetail(ProductInvoiceDetail ProductInvoiceDetail);
    }
}
