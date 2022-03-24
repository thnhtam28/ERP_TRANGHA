
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoKhachHangNoQuaDinhMucViewModel
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public decimal? RemainingAmount { get; set; }
        public decimal? PriceLevel { get; set; }
        public decimal? ToTalPriceLevel { get; set; }
        public decimal? RatioPriceLevel { get; set; }

    }
}