using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwSalesReturnsDetailMap : EntityTypeConfiguration<vwSalesReturnsDetail>
    {
        public vwSalesReturnsDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("vwSale_SalesReturnsDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.SalesReturnsId).HasColumnName("SalesReturnsId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ProductInvoiceDetailId).HasColumnName("ProductInvoiceDetailId");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.UnitProduct).HasColumnName("UnitProduct");

            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode");
            this.Property(t => t.ProductGroup).HasColumnName("ProductGroup");
            this.Property(t => t.Manufacturer).HasColumnName("Manufacturer");
            this.Property(t => t.LoCode).HasColumnName("LoCode");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.NgayTao).HasColumnName("NgayTao");
            this.Property(t => t.NguoiTao).HasColumnName("NguoiTao");
        }
    }
}
