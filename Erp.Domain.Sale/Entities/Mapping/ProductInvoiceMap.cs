using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class ProductInvoiceMap : EntityTypeConfiguration<ProductInvoice>
    {
        public ProductInvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code).HasMaxLength(20);
            this.Property(t => t.ShipName).HasMaxLength(50);
            this.Property(t => t.ShipAddress).HasMaxLength(200);
            this.Property(t => t.ShipWardId).HasMaxLength(50);
            this.Property(t => t.ShipDistrictId).HasMaxLength(50);
            this.Property(t => t.ShipCityId).HasMaxLength(50);
            this.Property(t => t.Status).HasMaxLength(50);
            this.Property(t => t.Note).HasMaxLength(300);


            // Table & Column Mappings
            this.ToTable("Sale_ProductInvoice");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.TaxFee).HasColumnName("TaxFee");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.DiscountAmount).HasColumnName("DiscountAmount");
            this.Property(t => t.PaidAmount).HasColumnName("PaidAmount");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ShipName).HasColumnName("ShipName");
            this.Property(t => t.ShipPhone).HasColumnName("ShipPhone");
            this.Property(t => t.ShipAddress).HasColumnName("ShipAddress");
            this.Property(t => t.ShipWardId).HasColumnName("ShipWardId");
            this.Property(t => t.ShipDistrictId).HasColumnName("ShipDistrictId");
            this.Property(t => t.ShipCityId).HasColumnName("ShipCityId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");

            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.CancelReason).HasColumnName("CancelReason");
            this.Property(t => t.PaymentMethod).HasColumnName("PaymentMethod");
            this.Property(t => t.CodeInvoiceRed).HasColumnName("CodeInvoiceRed");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.NextPaymentDate).HasColumnName("NextPaymentDate");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.DoanhThu).HasColumnName("DoanhThu");
            this.Property(t => t.Discount_VIP).HasColumnName("Discount_VIP");
            this.Property(t => t.Discount_KM).HasColumnName("Discount_KM");
            this.Property(t => t.Discount_DB).HasColumnName("Discount_DB");
            this.Property(t => t.CountForBrand).HasColumnName("CountForBrand");
            this.Property(t => t.ProductInvoiceId_Return).HasColumnName("ProductInvoiceId_Return");
        }
    }
}
