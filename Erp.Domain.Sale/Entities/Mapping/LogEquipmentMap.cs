using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class LogEquipmentMap : EntityTypeConfiguration<LogEquipment>
    {
        public LogEquipmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
                        this.Property(t => t.Note).HasMaxLength(300);
            this.Property(t => t.Status).HasMaxLength(100);


            // Table & Column Mappings
            this.ToTable("Sale_LogEquipment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.EquipmentId).HasColumnName("EquipmentId");
            this.Property(t => t.SchedulingHistoryId).HasColumnName("SchedulingHistoryId");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Status).HasColumnName("Status");

        }
    }
}
