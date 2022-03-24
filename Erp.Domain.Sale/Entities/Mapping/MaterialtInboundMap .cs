using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class MaterialInboundMap : EntityTypeConfiguration<MaterialInbound>
    {
        public MaterialInboundMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Note).HasMaxLength(300);
            this.Property(t => t.Type).HasMaxLength(20);


            // Table & Column Mappings
            this.ToTable("Sale_MaterialInbound");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.WarehouseKeeperId).HasColumnName("WarehouseKeeperId");
            this.Property(t => t.WarehouseDestinationId).HasColumnName("WarehouseDestinationId");
            this.Property(t => t.Type).HasColumnName("Type");

            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.IsDone).HasColumnName("IsDone");
            this.Property(t => t.BranchId).HasColumnName("BranchId");


            this.Property(t => t.PhysicalInventoryId).HasColumnName("PhysicalInventoryId");
            this.Property(t => t.MaterialOutboundId).HasColumnName("MaterialOutboundId");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");
            this.Property(t => t.CreatedStaffId).HasColumnName("CreatedStaffId");
        }
    }
}
