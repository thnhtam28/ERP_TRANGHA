//using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class PhysicalInventoryViewModel
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Code { get; set; }
        public Nullable<int> WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string Manufacturer { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public int? TotalBreak { get; set; }
        public int? TotalLost { get; set; }
        public int? TotalProductCheck { get; set; }
        public int? BranchId { get; set; }
        public bool? IsExchange { get; set; }
        public string ProductInboundCode { get; set; }
        public string ProductOutboundCode { get; set; }
        public string CancelReason { get; set; }
        public List<PhysicalInventoryDetailViewModel> DetailList { get; set; }
    }
}