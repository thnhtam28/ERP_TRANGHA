using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwInventoryMaterialMap : EntityTypeConfiguration<vwInventoryMaterial>
    {
        public vwInventoryMaterialMap()
        {
            this.HasKey(t => new { t.WarehouseId, t.MaterialId });            
            // Properties            

            // Table & Column Mappings
            this.ToTable("vwSale_InventoryMaterial");

            this.Property(t => t.MaterialId).HasColumnName("MaterialId");
            this.Property(t => t.ProductGroup).HasColumnName("ProductGroup");
            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode");
            this.Property(t => t.MaterialName).HasColumnName("MaterialName");
            this.Property(t => t.MaterialCode).HasColumnName("MaterialCode");
            this.Property(t => t.MaterialBarcode).HasColumnName("MaterialBarcode");
            this.Property(t => t.MaterialUnit).HasColumnName("MaterialUnit");
            this.Property(t => t.MaterialMinInventory).HasColumnName("MaterialMinInventory");
            this.Property(t => t.WarehouseId).HasColumnName("WarehouseId");
            this.Property(t => t.WarehouseName).HasColumnName("WarehouseName");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.LoCode).HasColumnName("LoCode");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Image_Name).HasColumnName("Image_Name");
            this.Property(t => t.CBTK).HasColumnName("CBTK");
            this.Property(t => t.IsSale).HasColumnName("IsSale");
            this.Property(t => t.MaterialManufacturer).HasColumnName("MaterialManufacturer");
            this.Property(t => t.MaterialPriceInbound).HasColumnName("MaterialPriceInbound");
            this.Property(t => t.MaterialPriceOutbound).HasColumnName("MaterialPriceOutbound");
           
        }
    }
}
