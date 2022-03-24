using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwMaterialMap : EntityTypeConfiguration<vwMaterial>
    {
        public vwMaterialMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("vwSale_Material");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode");
            this.Property(t => t.MinInventory).HasColumnName("MinInventory");
            this.Property(t => t.Barcode).HasColumnName("Barcode");
            this.Property(t => t.Image_Name).HasColumnName("Image_Name");
            this.Property(t => t.ProductGroup).HasColumnName("ProductGroup");
            this.Property(t => t.Manufacturer).HasColumnName("Manufacturer");
            this.Property(t => t.Size).HasColumnName("Size");
            this.Property(t => t.PriceInbound).HasColumnName("PriceInbound");
            this.Property(t => t.PriceOutBound).HasColumnName("PriceOutBound");
            this.Property(t => t.QuantityTotalInventory).HasColumnName("QuantityTotalInventory");
            this.Property(t => t.ProductGroupName).HasColumnName("ProductGroupName");
        }
    }
}
