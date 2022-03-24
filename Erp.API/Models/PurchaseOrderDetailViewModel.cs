//using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class PurchaseOrderDetailViewModel
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public int? AssignedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CategoryCode { get; set; }
        public Nullable<int> PurchaseOrderId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceTemp { get; set; }
        public Nullable<int> Quantity { get; set; }
        public int? QuantityInInventory { get; set; }
        public int? DisCount { get; set; }
        public int? DisCountAmount { get; set; }
        public string Unit { get; set; }
        public int OrderNo { get; set; }
        public int? QuantityRemaining { get; set; }
        public string ProductGroup { get; set; }
        public string PurchaseOrderCode { get; set; }
        public bool IsArchive { get; set; }
        public System.DateTime PurchaseOrderDate { get; set; }
        public decimal Amount { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public string ProductSize { get; set; }
        public string ProductType { get; set; }
        public string ProductMaterial { get; set; }
        public string ProductManufacturer { get; set; }
        public string ProductColor { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }
        public string SerialNumber { get; set; }
        public int? ParentId { get; set; }
        public string WarehouseDestinationCode { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainAmount { get; set; }
        public List<PurchaseOrderDetailViewModel> ServiceList { get; set; }
        public Nullable<System.DateTime> AssignedDate { get; set; }
        public string AssignStatus { get; set; }
        public string AssignedNote { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public string AssignedUserName { get; set; }
        public decimal? PriceService { get; set; }
        public string Image { get; set; }
        public decimal? PriceLabor { get; set; }
        public List<AssignServiceViewModel> AssignService { get; set; }
        public List<string> ListImageUpload { get; set; }

        public bool? IsApproved { get; set; }
        public bool? IsPayment { get; set; }
        public int? PaymentId { get; set; }

        public bool? IsPurchaseReturn { get; set; }
        public string ProductImageList { get; set; }
    }
}