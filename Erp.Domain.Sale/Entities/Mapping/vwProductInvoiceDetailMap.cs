using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwProductInvoiceDetailMap : EntityTypeConfiguration<vwProductInvoiceDetail>
    {
        public vwProductInvoiceDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            

            // Table & Column Mappings
            this.ToTable("vwSale_ProductInvoiceDetail");
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
            this.Property(t => t.ProductType).HasColumnName("ProductType");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode");
            this.Property(t => t.ProductInvoiceCode).HasColumnName("ProductInvoiceCode");
            this.Property(t => t.ProductGroup).HasColumnName("ProductGroup");
            this.Property(t => t.ProductInvoiceDate).HasColumnName("ProductInvoiceDate");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Manufacturer).HasColumnName("Manufacturer");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.IsCombo).HasColumnName("IsCombo");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CustomerPhone).HasColumnName("CustomerPhone");
            //abc
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");

            this.Property(t => t.LoCode).HasColumnName("LoCode");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.DistrictId).HasColumnName("DistrictId");
            this.Property(t => t.ProvinceName).HasColumnName("ProvinceName");
            this.Property(t => t.DistrictName).HasColumnName("DistrictName");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Day).HasColumnName("Day");
            this.Property(t => t.ProductImage).HasColumnName("ProductImage");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.DiscountAmount).HasColumnName("DiscountAmount");
            this.Property(t => t.Origin).HasColumnName("Origin");
            this.Property(t => t.ManagerStaffId).HasColumnName("ManagerStaffId");
            this.Property(t => t.Status).HasColumnName("Status");
            //this.Property(t => t.ProductOutboundCode).HasColumnName("ProductOutboundCode");
            //this.Property(t => t.ProductOutboundId).HasColumnName("ProductOutboundId");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.is_TANG).HasColumnName("is_TANG");
            this.Property(t => t.SOLAN_TANG_DV).HasColumnName("SOLAN_TANG_DV");
            this.Property(t => t.CountForBrand).HasColumnName("CountForBrand");
            this.Property(t => t.SOHOADON).HasColumnName("SOHOADON");
            this.Property(t => t.Tiensaugiam).HasColumnName("Tiensaugiam");

        }
    }
}
