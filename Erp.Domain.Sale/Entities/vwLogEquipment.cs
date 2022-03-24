using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwLogEquipment
    {
        public vwLogEquipment()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }

        public Nullable<int> EquipmentId { get; set; }
        public Nullable<int> SchedulingHistoryId { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string StaffEquipmentName { get; set; }
        public string StaffEquipmentCode { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string EquimentGroup { get; set; }
    }
}
