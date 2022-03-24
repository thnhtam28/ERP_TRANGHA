
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class BanGiaoTienViewModel
    {
        public int? SalerId { get; set; }
        public string CreatedDate { get; set; }
        public string FullName { get; set; }
        public decimal? AmountTransfer { get; set; }
        public decimal? AmountReceipt { get; set; }
        public string Status { get; set; }

    }
}