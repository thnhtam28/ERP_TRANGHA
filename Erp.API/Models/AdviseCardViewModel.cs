
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class AdviseCardViewModel
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

        public Nullable<int> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Code { get; set; }
        public Nullable<int> CounselorId { get; set; }
        public string Note { get; set; }
        public bool IsActived { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string CounselorName { get; set; }
        public string BranchCode { get; set; }

        public string CreatedUserCode { get; set; }

        public string CounselorCode { get; set; }
        public string Type { get; set; }

        public List<CategoryViewModel> ListAdviseType { get; set; }
    }
}