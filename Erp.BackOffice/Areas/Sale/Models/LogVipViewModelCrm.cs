using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class LogVipViewModelCrm
    {
        public LogVipViewModelCrm()
        {
        }

        public int Id { get; set; }


        public string Chude { get; set; }
        public string Mota { get; set; }
        public string Thoigianhen { get; set; }
        public string Hoanthanh { get; set; }



    }
}