
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoGiamGiaSauBanHangViewModel
    {
        public string ProductImage { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime? SalesDiscountDate { get; set; }
        public string SalesDiscountCode { get; set; }
        public string CustomerCode { get; set; }
        public string ProductInvoiceCode { get; set; }
        public int? ProductInvoiceId { get; set; }
        public int? SalesDiscountId { get; set; }
        public int? Quantity { get; set; }
        //public int? DisCount { get; set; }
        public decimal? Price { get; set; }
        public string ProductUnit { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainAmount { get; set; }
        public decimal? ProductInvoicePrice { get; set; }
    }
}