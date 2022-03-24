using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Entities
{
    public class StaffEquipment
    {
        public StaffEquipment()
        {

        }
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> InspectionDate { get; set; }
        public string Group { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> RoomId { get; set; }
        public Nullable<bool> StatusStaffMade { get; set; }
        
    }
}
