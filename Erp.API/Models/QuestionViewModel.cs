using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class QuestionViewModel
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
        public string Name { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public Nullable<bool> IsActivated { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public List<AnswerViewModel> DetailList { get; set; }
    }
}