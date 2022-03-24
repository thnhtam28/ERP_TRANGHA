//using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class ProductInvoiceDetailViewModel
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Nullable<int> ProductInvoiceId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceTest { get; set; }
        public Nullable<int> Quantity { get; set; }
        public int? PromotionId { get; set; }
        public int? PromotionDetailId { get; set; }
        public double? PromotionValue { get; set; }
        public int? QuantityInInventory { get; set; }
        public string Unit { get; set; }
        public string CategoryCode { get; set; }
        public string ProductType { get; set; }
        public int? DisCount { get; set; }
        public int? DisCountAmount { get; set; }
        public string ProductInvoiceCode { get; set; }
        public int OrderNo { get; set; }
        public List<WarehouseLocationItemViewModel> ListWarehouseLocationItemViewModel { get; set; }
        public string ProductGroup { get; set; }
        public string ProductGroupName { get; set; }
        public string LoCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? CheckPromotion { get; set; }
        public bool IsReturn { get; set; }
        public int? QuantitySaleReturn { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
        public DateTime? ProductInvoiceDate { get; set; }
        public bool isArchive { get; set; }
        public string CustomerName { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainAmount { get; set; }
        public string DepositCode { get; set; }
        public int? DepositReceiptId { get; set; }
    }
}