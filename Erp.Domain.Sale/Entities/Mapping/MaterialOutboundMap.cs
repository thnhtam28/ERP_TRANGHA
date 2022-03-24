using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class MaterialOutboundMap : EntityTypeConfiguration<MaterialOutbound>
    {
        public MaterialOutboundMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Note).HasMaxLength(300);
            this.Property(t => t.Type).HasMaxLength(20);


            // Table & Column Mappings
            this.ToTable("Sale_MaterialOutbound");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.IsDone).HasColumnName("IsDone");

            this.Property(t => t.WarehouseSourceId).HasColumnName("WarehouseSourceId");
            this.Property(t => t.WarehouseKeeperId).HasColumnName("WarehouseKeeperId");
            
            
            this.Property(t => t.WarehouseDestinationId).HasColumnName("WarehouseDestinationId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.ReasonManual).HasColumnName("ReasonManual");
            this.Property(t => t.PhysicalInventoryId).HasColumnName("PhysicalInventoryId");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");
            this.Property(t => t.CreatedStaffId).HasColumnName("CreatedStaffId");
        }
    }
}
