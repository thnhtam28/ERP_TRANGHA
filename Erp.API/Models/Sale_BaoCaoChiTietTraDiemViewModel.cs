
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoChiTietTraDiemViewModel
    {
        public string Code { get; set; }
public string CustomerName { get; set; }
public string CustomerAddress { get; set; }
public string Phone { get; set; }
public string Note { get; set; }
public string Point { get; set; }

    }
}