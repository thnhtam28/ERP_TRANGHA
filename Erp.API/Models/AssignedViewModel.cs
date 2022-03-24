//using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class AssignedViewModel
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

        public Nullable<System.DateTime> AssignedDate { get; set; }

        public string AssignedUserName { get; set; }
        public Nullable<int> PurchaseOrderDetailId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsPayment { get; set; }
        public decimal? LaborAmount { get; set; }
        public decimal? LaborPaidAmount { get; set; }
        public decimal? LaborRemainAmount { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Image { get; set; }
        public List<AssignServiceViewModel> AssignService { get; set; }
    }
}