
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoBanHangTheoChiNhanhViewModel
    {
        public string BranchName { get; set; }
public decimal? Revenue { get; set; }
public decimal? OtherRevenue { get; set; }
public decimal? TotalRevenue { get; set; }

    }
}