using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class MaterialInboundDetailMap : EntityTypeConfiguration<MaterialInboundDetail>
    {
        public MaterialInboundDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Unit).HasMaxLength(50);


            // Table & Column Mappings
            this.ToTable("Sale_MaterialInboundDetail");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");


            this.Property(t => t.MaterialInboundId).HasColumnName("MaterialInboundId");
            this.Property(t => t.MaterialId).HasColumnName("MaterialId");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.LoCode).HasColumnName("LoCode");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Price).HasColumnName("Price");
        }
    }
}
