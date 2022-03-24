using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwSchedulingHistoryMap : EntityTypeConfiguration<vwSchedulingHistory>
    {
        public vwSchedulingHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Status).HasMaxLength(150);
            this.Property(t => t.Code).HasMaxLength(100);
            this.Property(t => t.Note).HasMaxLength(500);


            // Table & Column Mappings
            this.ToTable("vwSale_SchedulingHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.NameNV).HasColumnName("NameNV");
            this.Property(t => t.IdNV).HasColumnName("IdNV");
            this.Property(t => t.TimeExecution).HasColumnName("TimeExecution");
            this.Property(t => t.InquiryCardId).HasColumnName("InquiryCardId");
            this.Property(t => t.WorkDay).HasColumnName("WorkDay");
            this.Property(t => t.TotalMinute).HasColumnName("TotalMinute");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.ExpectedEndDate).HasColumnName("ExpectedEndDate");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.RoomId).HasColumnName("RoomId");
            this.Property(t => t.BedId).HasColumnName("BedId");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Name_Bed).HasColumnName("Name_Bed");
            this.Property(t => t.Name_Room).HasColumnName("Name_Room");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.BranchCode).HasColumnName("BranchCode");
            this.Property(t => t.CreatedUserName).HasColumnName("CreatedUserName");
            this.Property(t => t.CreatedUserCode).HasColumnName("CreatedUserCode");
            this.Property(t => t.InquiryCardCode).HasColumnName("InquiryCardCode");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.RoomName).HasColumnName("RoomName");
            this.Property(t => t.FloorName).HasColumnName("FloorName");
            this.Property(t => t.CustomerImage).HasColumnName("CustomerImage");
            this.Property(t => t.ExpectedWorkDay).HasColumnName("ExpectedWorkDay");
            this.Property(t => t.EquimentGroup).HasColumnName("EquimentGroup");
            //hoapd them
            this.Property(t => t.TargetModule).HasColumnName("TargetModule");
            this.Property(t => t.TargetId).HasColumnName("TargetId");
            this.Property(t => t.WorkDaystr).HasColumnName("WorkDaystr");
            this.Property(t => t.moretime).HasColumnName("moretime");


        }
    }
}
