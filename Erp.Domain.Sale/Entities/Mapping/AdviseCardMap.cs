using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class AdviseCardMap : EntityTypeConfiguration<AdviseCard>
    {
        public AdviseCardMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code).HasMaxLength(50);
            this.Property(t => t.Note).HasMaxLength(500);


            // Table & Column Mappings
            this.ToTable("Sale_AdviseCard");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CounselorId).HasColumnName("CounselorId");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsActived).HasColumnName("IsActived");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Type).HasColumnName("Type");
        }
    }
}
