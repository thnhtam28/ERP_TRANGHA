
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoCongNoTheoNhanVienViewModel
    {
        public string FullName { get; set; }
        public decimal? TongNoDauKy { get; set; }
        public decimal? TangGiuaKy { get; set; }
        public decimal? GiamGiuaKy { get; set; }
        public decimal? TongNoCuoiKy { get; set; }
        public int? SalerId { get; set; }
    }
}