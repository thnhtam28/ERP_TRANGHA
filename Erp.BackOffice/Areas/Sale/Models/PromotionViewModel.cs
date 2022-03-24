using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class PromotionViewModel
    {
        public int Id { get; set; }

        [Display(Name = "IsDeleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "CreatedUser", ResourceType = typeof(Wording))]
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(Wording))]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "ModifiedUser", ResourceType = typeof(Wording))]
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }

        [Display(Name = "ModifiedDate", ResourceType = typeof(Wording))]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "AssignedUserId", ResourceType = typeof(Wording))]
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [StringLength(100, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError", ErrorMessage = null)]
        [Display(Name = "Name", ResourceType = typeof(Wording))]
        public string Name { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Wording))]
        public string Code { get; set; }

        [Display(Name = "PromotionValue", ResourceType = typeof(Wording))]
        public double? PercentValue { get; set; }

        [Display(Name = "StartDate", ResourceType = typeof(Wording))]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Display(Name = "EndDate", ResourceType = typeof(Wording))]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Wording))]
        public string Note { get; set; }

        [Display(Name = "IsAllProduct", ResourceType = typeof(Wording))]
        public bool? IsAllProduct { get; set; }

        [Display(Name = "IsAllService", ResourceType = typeof(Wording))]
        public bool? IsAllService { get; set; }

        public List<PromotionDetailViewModel> DetailList { get; set; }

    }
}