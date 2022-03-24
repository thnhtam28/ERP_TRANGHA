using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwProductSample
    {
        public vwProductSample()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
       

        public string Code { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
    }
}
