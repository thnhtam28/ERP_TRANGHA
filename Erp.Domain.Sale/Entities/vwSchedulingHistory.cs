using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwSchedulingHistory
    {
        public vwSchedulingHistory()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        public string TimeExecution { get; set; }
        public Nullable<int> InquiryCardId { get; set; }
        public Nullable<System.DateTime> WorkDay { get; set; }
        public Nullable<int> TotalMinute { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> ExpectedEndDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public Nullable<int> RoomId { get; set; }
        public Nullable<int> BedId { get; set; }
        public string Note { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int>IdNV { get; set; }
        public string BranchName { get; set; }
        public string NameNV { get; set; }
        public string BranchCode { get; set; }
        public string Name_Bed { get; set; }
        public string Name_Room { get; set; }
        public string CreatedUserName { get; set; }
        public string CreatedUserCode { get; set; }
        public string InquiryCardCode { get; set; }
        public string Type { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string RoomName { get; set; }
        public string FloorName { get; set; }
        public string CustomerImage { get; set; }
        public Nullable<System.DateTime> ExpectedWorkDay { get; set; }
        public string EquimentGroup { get; set; }
        public string TargetModule { get; set; }
        public Nullable<int> TargetId { get; set; }
        public string WorkDaystr { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public Nullable<int> executionTime { get; set; }
        public Nullable<int> moretime { get; set; }
    }
}
