using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class BaoCaoLaiLoTrongKinhDoanhViewModel
    {

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string ProductImage { get; set; }
        public int Quantity_Invoice { get; set; }
        public decimal? Price_Invoice { get; set; }
        public decimal? DisCountAmount_Invoice { get; set; }
        public decimal? Amount_Invoice { get; set; }

        public decimal? Price_Inbound { get; set; }
        public decimal? Amount_Inbound { get; set; }
        public decimal? Profit { get; set; }
    }
}