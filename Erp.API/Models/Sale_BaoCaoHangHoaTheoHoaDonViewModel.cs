
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoHangHoaTheoHoaDonViewModel
    {
        public string ProductInvoiceCode { get; set; }
        public string CustomerName { get; set; }
        public decimal? Price { get; set; }
        public decimal? DisCountAmount { get; set; }
        public decimal? TaxValue { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainAmount { get; set; }
        public int Id { get; set; }

    }
}