
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoTongHopTichDiemViewModel
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Phone { get; set; }
        public int? Beginning { get; set; }
        public int? Increase { get; set; }
        public int? Reduction { get; set; }
        public int? Ending { get; set; }

    }
}