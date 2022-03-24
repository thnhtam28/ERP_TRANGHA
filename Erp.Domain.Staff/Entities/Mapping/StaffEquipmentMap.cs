using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Entities.Mapping
{
    public class StaffEquipmentMap: EntityTypeConfiguration<StaffEquipment>
    {
        public StaffEquipmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(50);
            this.Property(t=>t.Code).HasMaxLength(50);


            // Table & Column Mappings
            this.ToTable("Staff_Equipment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.RoomId).HasColumnName("RoomId");
            this.Property(t => t.InspectionDate).HasColumnName("InspectionDate");
            this.Property(t => t.Group).HasColumnName("Group");
            this.Property(t => t.StatusStaffMade).HasColumnName("StatusStaffMade");
        }
    }
}
