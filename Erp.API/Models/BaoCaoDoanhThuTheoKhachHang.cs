using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class BaoCaoDoanhThuTheoKhachHang
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public decimal Amount { get; set; }
    }
}