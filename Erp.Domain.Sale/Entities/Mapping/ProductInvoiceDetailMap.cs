using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class ProductInvoiceDetailMap : EntityTypeConfiguration<ProductInvoiceDetail>
    {
        public ProductInvoiceDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties            

            // Table & Column Mappings
            this.ToTable("Sale_ProductInvoiceDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Unit).HasColumnName("Unit");          

            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.DiscountAmount).HasColumnName("DiscountAmount");
            this.Property(t => t.LoCode).HasColumnName("LoCode");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.is_TANG).HasColumnName("is_TANG");
            this.Property(t => t.SOLAN_TANG_DV).HasColumnName("SOLAN_TANG_DV");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
        }
    }
}
