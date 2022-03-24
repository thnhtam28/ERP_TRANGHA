
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoBanHangTheoNhanVienViewModel
    {
        public string SalerFullName { get; set; }
        public decimal? DauKy { get; set; }
        public decimal? TrongKy { get; set; }
        public decimal? CuoiKy { get; set; }

    }
}