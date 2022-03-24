using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class AdviseCardDetailMap : EntityTypeConfiguration<AdviseCardDetail>
    {
        public AdviseCardDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("Sale_AdviseCardDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.AdviseCardId).HasColumnName("AdviseCardId");
            this.Property(t => t.QuestionId).HasColumnName("QuestionId");
            this.Property(t => t.TargetId).HasColumnName("TargetId");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.TargetModule).HasColumnName("TargetModule");

        }
    }
}
