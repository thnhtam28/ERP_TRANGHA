using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class BaoCaoThuChiTheoHinhThucThanhToanViewModel
    {
        public string PaymentMethod { get; set; }

        public decimal? Amount_Payment { get; set; }
        public decimal? Amount_Receipt { get; set; }
        public decimal? Amount_Total { get; set; }
        public decimal? FirstAmount { get; set; }
        public decimal? LastAmount { get; set; }
    }
}