using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwStaffMadeMap : EntityTypeConfiguration<vwStaffMade>
    {
        public vwStaffMadeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
                        this.Property(t => t.Status).HasMaxLength(50);
            this.Property(t => t.Note).HasMaxLength(500);


            // Table & Column Mappings
            this.ToTable("vwSale_StaffMade");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SchedulingHistoryId).HasColumnName("SchedulingHistoryId");
            this.Property(t => t.UserCode).HasColumnName("UserCode");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.ProfileImage).HasColumnName("ProfileImage");
            this.Property(t => t.WorkDay).HasColumnName("WorkDay");
            this.Property(t => t.BranchId).HasColumnName("BranchId");

            this.Property(t => t.RoomName).HasColumnName("RoomName");
            this.Property(t => t.FloorName).HasColumnName("FloorName");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.SchedulingCode).HasColumnName("SchedulingCode");
            this.Property(t => t.SchedulingStatus).HasColumnName("SchedulingStatus");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.CustomerImage).HasColumnName("CustomerImage");

            this.Property(t => t.ExpectedWorkDay).HasColumnName("ExpectedWorkDay");
            this.Property(t => t.TotalMinute).HasColumnName("TotalMinute");
            this.Property(t => t.ExpectedEndDate).HasColumnName("ExpectedEndDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Name_Bed).HasColumnName("Name_Bed");
    }
    }
}
