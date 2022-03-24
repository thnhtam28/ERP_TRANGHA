//using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class PurchaseOrderViewModel
    {
        public PurchaseOrderViewModel()
        {
            Discount = 0;
            TaxFee = 0;
            TotalAmount = 0;
        }

        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }
        public string Code { get; set; }
        public decimal? TotalAmount { get; set; }
        public double? TaxFee { get; set; }
        public double? Discount { get; set; }
        public string DiscountCode { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Phone { get; set; }
        public bool IsArchive { get; set; }
        public int? BranchId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public string CancelReason { get; set; }
        public string BarCode { get; set; }
        public string Type { get; set; }
        public int? WarehouseSourceId { get; set; }
        public int? WarehouseDestinationId { get; set; }
        public string WarehouseDestinationCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string WarehouseSourceName { get; set; }
        public string WarehouseDestinationName { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public DateTime? NextPaymentDate_Temp { get; set; }
        public string ImageList { get; set; }
        public string BrandName { get; set; }
        public int? ProductInboundId { get; set; }
        public string ProductInboundCode { get; set; }
        public bool AllowEdit { get; set; }
        public List<PurchaseOrderDetailViewModel> DetailList { get; set; }
        public List<PurchaseOrderDetailViewModel> GroupProduct { get; set; }
        public int? QuantityCodeSaleReturns { get; set; }

        public List<PurchaseOrderPaymentViewModel> PaymentList { get; set; }
        public List<string> ListImg { get; set; }
        public int? CustomerId { get; set; }
        public bool? PaymentType { get; set; }
        public List<PaymentPendingViewModel> PaymentPending { get; set; }
        public decimal? PayTotalAmount { get; set; }
        public bool IsFromApp { get; set; }
    }
    public class PaymentPendingViewModel
    {
        public bool? IsCheck { get; set; }
        public int Id { get; set; }
        public string LoaiCT { get; set; }
    }
    public class PurchaseOrderPaymentViewModel
    {
        public string Name { get; set; }
        public int? PurchaseOrderDetailId { get; set; }
        public decimal Amount { get; set; }

        public List<PurchaseOrderSubPaymentViewModel> SubList { get; set; }
    }

    public class PurchaseOrderSubPaymentViewModel
    {
        public int? ProductInvoiceDetailId { get; set; }
        public decimal Amount { get; set; }

    }
}