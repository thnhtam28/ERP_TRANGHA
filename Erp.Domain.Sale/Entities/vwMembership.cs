using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwMembership
    {
        public vwMembership()
        {

        }

        public Int64 Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        //public string Name { get; set; }

        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Status { get; set; }
        public Nullable<Int64> TargetId { get; set; }
        public string TargetModule { get; set; }
        public string Code { get; set; }
        public string TargetCode { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> ExpiryDateOld { get; set; }
        public Nullable<int> BranchId { get; set; }

        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string CreatedUserName { get; set; }
        public string CreatedUserCode { get; set; }
        public string ManagerName { get; set; }
        public string Type { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string CustomerImage { get; set; }
        public string CustomerPhone { get; set; }

        public Nullable<int> Is_extend { get; set; }

        public string idcu { get; set; }
        public Nullable<int> solandadung { get; set; }
        public Nullable<int> solantratra { get; set; }
        public Nullable<int> TongLanCSD { get; set; }

        public Int64 IdParent { get; set; }




    }
}
