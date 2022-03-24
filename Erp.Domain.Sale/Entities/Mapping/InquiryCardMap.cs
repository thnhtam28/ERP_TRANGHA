using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class InquiryCardMap : EntityTypeConfiguration<InquiryCard>
    {
        public InquiryCardMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code).HasMaxLength(50);
                        this.Property(t => t.Note).HasMaxLength(1000);


            // Table & Column Mappings
            this.ToTable("Sale_InquiryCard");
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
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.SkinscanUserId).HasColumnName("SkinscanUserId");

        }
    }
}
