using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class WarehouseLocationItemViewModel
    {
        public int? Id { get; set; }

        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }
        public string SN { get; set; }
        public string Shelf { get; set; }
        public string Floor { get; set; }
        public string Position { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Nullable<bool> IsOut { get; set; }
        public Nullable<int> WarehouseId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> ProductInboundId { get; set; }
        public Nullable<int> ProductInboundDetailId { get; set; }
        public string WarehouseName { get; set; }
        public string ProductName { get; set; }
        public string ProductBarCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductInboundCode { get; set; }
        public int? BranchId { get; set; }
        public string LoCode { get; set; }
    }
}