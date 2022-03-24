
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoBanHangTheoThangViewModel
    {
        public int? Month { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainingAmount { get; set; }

    }
}