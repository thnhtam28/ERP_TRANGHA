
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoThongkeDiemKhachHangViewModel
    {
        public string CustomerCode { get; set; }
public string CustomerName { get; set; }
public string CustomerAddress { get; set; }
public int? Beginning { get; set; }
public int? Miding { get; set; }
public int? PayMiding { get; set; }
public int? Ending { get; set; }

    }
}