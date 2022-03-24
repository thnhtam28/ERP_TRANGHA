using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class InventoryMaterialMap : EntityTypeConfiguration<InventoryMaterial>
    {
        public InventoryMaterialMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            

            // Table & Column Mappings
            this.ToTable("Sale_InventoryMaterial");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.WarehouseId).HasColumnName("WarehouseId");
            this.Property(t => t.MaterialId).HasColumnName("MaterialId");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.LoCode).HasColumnName("LoCode");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.BranchId).HasColumnName("BranchId");

        }
    }
}
