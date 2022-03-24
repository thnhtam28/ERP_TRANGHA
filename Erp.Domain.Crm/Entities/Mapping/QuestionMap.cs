using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(150);
            this.Property(t => t.Type).HasMaxLength(50);
            this.Property(t => t.Category).HasMaxLength(150);


            // Table & Column Mappings
            this.ToTable("Crm_Question");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.IsActivated).HasColumnName("IsActivated");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
        }
    }
}
