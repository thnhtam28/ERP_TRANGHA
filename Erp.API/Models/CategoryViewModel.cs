
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Erp.API.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? OrderNo { get; set; }
        public int? ParentId { get; set; }
        public string NameParent { get; set; }
        public IEnumerable<Category> ListCategory { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string NameCreateUser { get; set; }
        public int? ModifiedUserId { get; set; }
        public string NameModifiedUser { get; set; }
        public string Value { get; set; }
        public bool? Check { get; set; }
        public int Level { get; set; }

        public List<QuestionViewModel> ListQuestion { get; set; }
    }
}