using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwMaterialOutboundDetailMap : EntityTypeConfiguration<vwMaterialOutboundDetail>
    {
        public vwMaterialOutboundDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Unit).HasMaxLength(50);


            // Table & Column Mappings
            this.ToTable("vwSale_MaterialOutboundDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.MaterialOutboundId).HasColumnName("MaterialOutboundId");
            this.Property(t => t.MaterialId).HasColumnName("MaterialId");
            this.Property(t => t.MaterialCode).HasColumnName("MaterialCode");
            this.Property(t => t.MaterialName).HasColumnName("MaterialName");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Unit).HasColumnName("Unit");
            
            this.Property(t => t.MaterialOutboundCode).HasColumnName("MaterialOutboundCode");
            this.Property(t => t.WarehouseDestinationName).HasColumnName("WarehouseDestinationName");
            this.Property(t => t.WarehouseSourceName).HasColumnName("WarehouseSourceName");
            this.Property(t => t.WarehouseDestinationId).HasColumnName("WarehouseDestinationId");
            this.Property(t => t.WarehouseSourceId).HasColumnName("WarehouseSourceId");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.LoCode).HasColumnName("LoCode");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
        }
    }
}
