using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class BaoCaoChiTietTraHangNCCViewModel
    {
        //public int Id { get; set; }
        public int? BranchId { get; set; }
        public string CategoryCode { get; set; }
        //public string CustomerCode { get; set; }
        public int? SupplierId { get; set; }
        //public int? SupplierId { get; set; }
        //public string SupplierName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public int? DisCount { get; set; }
        public int? DisCountAmount { get; set; }
        public string Manufacturer { get; set; }
        public decimal? Price { get; set; }
        public string ProductCode { get; set; }
        public int? ProductId { get; set; }
        public string ProductGroup { get; set; }
        public string PurchaseOrderCode { get; set; }
        public DateTime? PurchaseReturnsDate { get; set; }
        public DateTime? PurchaseOrderDate { get; set; }
        public string PurchaseReturnsCode { get; set; }
        //public DateTime? SaleReturnDate { get; set; }
        public int? PurchaseReturnsId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public int? Quantity { get; set; }
        //public string SalerReturnName { get; set; }
        //public int? SalerId { get; set; }
        //public string ShipAddress { get; set; }
        public string ProductUnit { get; set; }
        public string ProductImage { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainAmount { get; set; }
    }
}