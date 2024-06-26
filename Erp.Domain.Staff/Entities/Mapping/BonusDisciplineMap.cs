using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Entities.Mapping
{
    public class BonusDisciplineMap : EntityTypeConfiguration<BonusDiscipline>
    {
        public BonusDisciplineMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Category).HasMaxLength(150);
            this.Property(t => t.Formality).HasMaxLength(150);
            this.Property(t => t.Reason).HasMaxLength(350);
            this.Property(t => t.Note).HasMaxLength(350);


            // Table & Column Mappings
            this.ToTable("Staff_BonusDiscipline");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Formality).HasColumnName("Formality");
            this.Property(t => t.Reason).HasColumnName("Reason");
            this.Property(t => t.DayDecision).HasColumnName("DayDecision");
            this.Property(t => t.DayEffective).HasColumnName("DayEffective");
            this.Property(t => t.PlaceDecisions).HasColumnName("PlaceDecisions");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.StaffId).HasColumnName("StaffId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Money).HasColumnName("Money");
        }
    }
}
