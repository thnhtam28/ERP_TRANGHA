
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoKetQuaBanHangTheoThangViewModel
    {
        public string ProductCode { get; set; }
public string ProductName { get; set; }
public int? Quantity { get; set; }
public decimal? AmountOut { get; set; }
public decimal? AmountIn { get; set; }
public decimal? InterestRate { get; set; }

    }
}