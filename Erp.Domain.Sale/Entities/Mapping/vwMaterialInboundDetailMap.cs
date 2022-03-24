using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwMaterialInboundDetailMap : EntityTypeConfiguration<vwMaterialInboundDetail>
    {
        public vwMaterialInboundDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            // Table & Column Mappings
            this.ToTable("vwSale_MaterialInboundDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.MaterialInboundId).HasColumnName("MaterialInboundId");
            this.Property(t => t.MaterialId).HasColumnName("MaterialId");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.LoCode).HasColumnName("LoCode");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.MaterialInboundCode).HasColumnName("MaterialInboundCode");
            this.Property(t => t.WarehouseDestinationId).HasColumnName("WarehouseDestinationId");
            this.Property(t => t.WarehouseDestinationName).HasColumnName("WarehouseDestinationName");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");
            this.Property(t => t.MaterialName).HasColumnName("MaterialName");
            this.Property(t => t.MaterialCode).HasColumnName("MaterialCode");
            this.Property(t => t.MaterialBarCode).HasColumnName("MaterialBarCode");
            this.Property(t => t.MaterialBarCode).HasColumnName("MaterialBarCode");
            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode");
            this.Property(t => t.ProductGroup).HasColumnName("ProductGroup");
            this.Property(t => t.Manufacturer).HasColumnName("Manufacturer");
            this.Property(t => t.MaterialUnit).HasColumnName("MaterialUnit");
            this.Property(t => t.Image_Name).HasColumnName("Image_Name");
            this.Property(t => t.MaterialMinInventory).HasColumnName("MaterialMinInventory");
        }
    }
}
