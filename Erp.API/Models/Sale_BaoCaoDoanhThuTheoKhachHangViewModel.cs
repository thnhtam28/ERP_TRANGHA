
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoDoanhThuTheoKhachHangViewModel
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public decimal? Amount { get; set; }

    }
}