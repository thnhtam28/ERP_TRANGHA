using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwStaffMade
    {
        public vwStaffMade()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }

        public Nullable<int> UserId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public Nullable<int> SchedulingHistoryId { get; set; }

        public string FullName { get; set; }
        public string UserCode { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<System.DateTime> WorkDay { get; set; }
        public Nullable<int> BranchId { get; set; }

        public string RoomName { get; set; }
        public string FloorName { get; set; }
        public string Type { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string SchedulingCode { get; set; }
        public string SchedulingStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerImage { get; set; }

        public Nullable<System.DateTime> ExpectedWorkDay { get; set; }
        public Nullable<int> TotalMinute { get; set; }
        public Nullable<System.DateTime> ExpectedEndDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Name_Bed { get; set; }
        
    }
}
