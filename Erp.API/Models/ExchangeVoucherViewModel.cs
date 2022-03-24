using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class ExchangeVoucherViewModel
    {
        public int Id { get; set; }
        public string LoaiCT { get; set; }
        public string MaCT { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsCheck { get; set; }
    }
}