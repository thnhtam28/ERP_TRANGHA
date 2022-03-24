using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwInquiryCard
    {
        public vwInquiryCard()
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

        public string TargetModule { get; set; }
        public Nullable<int> TargetId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> TotalMinute { get; set; }
        public Nullable<System.DateTime> WorkDay { get; set; }
        public string Note { get; set; }
        public Nullable<int> ManagerUserId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public Nullable<int> SkinscanUserId { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string CreateUserName { get; set; }
        public string CreateUserCode { get; set; }
        public string ManagerName { get; set; }
        public string ManagerCode { get; set; }
        public string SkinscanUserName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string TargetCode { get; set; }
        public string CustomerImage { get; set; }
        public string EquimentGroup { get; set; }
    }
}
