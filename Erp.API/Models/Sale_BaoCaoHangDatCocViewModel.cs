
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoHangDatCocViewModel
    {
        public string ProductCode { get; set; }
        public string Code { get; set; }
        public DateTime? VoucherDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TargetId { get; set; }
        public string TargetCode { get; set; }
        public string TargetName { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? Amount { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string Status { get; set; }
 
    }
}