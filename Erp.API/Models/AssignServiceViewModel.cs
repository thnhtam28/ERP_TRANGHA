//using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class AssignServiceViewModel
    {
        public int Id { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }

        public decimal? Price { get; set; }

        public Nullable<int> PurchaseOrderDetailId { get; set; }
        public int? ServiceOrderDetailId { get; set; }
        public Nullable<int> Percent { get; set; }
        public decimal? AdjustmentPrice { get; set; }
        public string AdjustmentType { get; set; }
        public Nullable<System.DateTime> AssignedDate { get; set; }
        public string AssignedNote { get; set; }
        public string AssignStatus { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public string ServiceName { get; set; }
        public decimal? PriceService { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public int? ServiceId { get; set; }
        
    }
}