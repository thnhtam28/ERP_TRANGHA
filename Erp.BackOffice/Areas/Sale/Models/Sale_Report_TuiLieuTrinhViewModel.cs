
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class Sale_Report_TuiLieuTrinhViewModel
    {
        public DateTime? CreatedDate { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int? Quantity { get; set; }

    }
}