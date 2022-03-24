using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwInquiryCardMap : EntityTypeConfiguration<vwInquiryCard>
    {
        public vwInquiryCardMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code).HasMaxLength(50);
                        this.Property(t => t.Note).HasMaxLength(1000);


            // Table & Column Mappings
            this.ToTable("vwSale_InquiryCard");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.Code).HasColumnName("Code");

            this.Property(t => t.TargetModule).HasColumnName("TargetModule");
            this.Property(t => t.TargetId).HasColumnName("TargetId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.TotalMinute).HasColumnName("TotalMinute");
            this.Property(t => t.WorkDay).HasColumnName("WorkDay");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ManagerUserId).HasColumnName("ManagerUserId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.SkinscanUserId).HasColumnName("SkinscanUserId");

            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.BranchCode).HasColumnName("BranchCode");
            this.Property(t => t.CreateUserName).HasColumnName("CreateUserName");
            this.Property(t => t.CreateUserCode).HasColumnName("CreateUserCode");
            this.Property(t => t.ManagerName).HasColumnName("ManagerName");
            this.Property(t => t.ManagerCode).HasColumnName("ManagerCode");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.SkinscanUserName).HasColumnName("SkinscanUserName");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.TargetCode).HasColumnName("TargetCode");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CustomerImage).HasColumnName("CustomerImage");
            this.Property(t => t.EquimentGroup).HasColumnName("EquimentGroup");
        }
    }
}
