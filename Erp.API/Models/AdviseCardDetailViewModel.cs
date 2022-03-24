
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class AdviseCardDetailViewModel
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

        public Nullable<int> AdviseCardId { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public Nullable<int> TargetId { get; set; }
        public string Note { get; set; }
        public string Content { get; set; }
        public string TargetModule { get; set; }
    }
}