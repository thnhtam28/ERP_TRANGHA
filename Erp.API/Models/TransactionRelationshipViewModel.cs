using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class TransactionRelationshipViewModel
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
        public string TransactionA { get; set; }
        public string TransactionB { get; set; }
        public string TransactionA_Module { get; set; }
        public string TransactionA_Name { get; set; }
        public string TransactionB_Module { get; set; }
        public string TransactionB_Name { get; set; }

    }
}