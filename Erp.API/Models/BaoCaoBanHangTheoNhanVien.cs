using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class BaoCaoBanHangTheoNhanVien
    {
        public int SalerId { get; set; }
        public string SalerFullName { get; set; }
        public decimal DauKy { get; set; }
        public decimal TrongKy { get; set; }
        public decimal CuoiKy { get; set; }
    }
}