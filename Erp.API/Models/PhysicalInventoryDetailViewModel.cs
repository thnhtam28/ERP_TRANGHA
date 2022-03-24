//using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class PhysicalInventoryDetailViewModel
    {
        public int Id { get; set; }

        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int PhysicalInventoryId { get; set; }
        public int ProductId { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int QuantityInInventory { get; set; }
        public int QuantityRemaining { get; set; }
        public int QuantityDiff { get; set; }
        public string Note { get; set; }
        public string ReferenceVoucher { get; set; }
    }
}