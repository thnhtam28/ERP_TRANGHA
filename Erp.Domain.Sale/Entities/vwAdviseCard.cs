using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwAdviseCard
    {
        public vwAdviseCard()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }

        public Nullable<int> CustomerId { get; set; }
        public string Code { get; set; }
        public Nullable<int> CounselorId { get; set; }
        public string Note { get; set; }
        public bool IsActived { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string BranchName { get; set; }
        public string CreatedUserName { get; set; }
        public string CounselorName { get; set; }
        public string BranchCode { get; set; }
        public string CreatedUserCode { get; set; }
        public string CounselorCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerAddress { get; set; }
        public Nullable<System.DateTime> CustomerBirthday { get; set; }
        public string Type { get; set; }
    }
}
