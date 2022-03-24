using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class ProductSampleMap : EntityTypeConfiguration<ProductSample>
    {
        public ProductSampleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Name).HasMaxLength(150);
            //            this.Property(t => t.Status).HasMaxLength(100);
            this.Property(t => t.Note).HasMaxLength(500);


            // Table & Column Mappings
            this.ToTable("Sale_ProductSample");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            //this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.BranchId).HasColumnName("BranchId");

        }
    }
}
