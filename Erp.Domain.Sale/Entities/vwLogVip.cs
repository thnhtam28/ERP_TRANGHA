using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwLogVip
    {
        public vwLogVip()
        {

        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        //public string Name { get; set; }

        public Nullable<int> CustomerId { get; set; }
        public string Status { get; set; }
        public string Ratings { get; set; }
        public Nullable<int> ApprovedUserId { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public int? Year { get; set; }
        public string LoyaltyPointName { get; set; }

        //public string CustomerName { get; set; }
        public bool is_approved { get; set; }
        public int? LoyaltyPointId { get; set; }
        public int? BranchId { get; set; }
        public string ApprovedUserName { get; set; }
    }
}
