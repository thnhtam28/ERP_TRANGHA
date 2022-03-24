using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Entities
{
    public class SalarySettingDetail_Staff
    {
        public SalarySettingDetail_Staff()
        {
            
        }


        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }

        public Nullable<int> StaffId { get; set; }
        public Nullable<int> SalarySettingId { get; set; }
        public Nullable<int> SalarySettingDetailId { get; set; }
        public double? DefaultValue { get; set; }


    }
}
