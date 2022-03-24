using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class AdviseCardDetail
    {
        public AdviseCardDetail()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }

        public Nullable<int> AdviseCardId { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public Nullable<int> TargetId { get; set; }
        public string Note { get; set; }
        public string Content { get; set; }
        public string TargetModule { get; set; }
    }
}
