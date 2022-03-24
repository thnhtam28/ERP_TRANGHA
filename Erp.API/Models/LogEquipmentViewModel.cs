using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class LogEquipmentViewModel
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }
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