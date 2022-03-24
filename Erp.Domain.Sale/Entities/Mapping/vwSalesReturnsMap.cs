using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwSalesReturnsMap : EntityTypeConfiguration<vwSalesReturns>
    {
        public vwSalesReturnsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code).HasMaxLength(20);
            this.Property(t => t.Status).HasMaxLength(50);
            this.Property(t => t.Note).HasMaxLength(300);
            this.Property(t => t.PaymentMethod).HasMaxLength(150);


            // Table & Column Mappings
            this.ToTable("vwSale_SalesReturns");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.TaxFee).HasColumnName("TaxFee");

            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.PaymentMethod).HasColumnName("PaymentMethod");
            this.Property(t => t.PaidAmount).HasColumnName("PaidAmount");
            this.Property(t => t.ProductInvoiceNewId).HasColumnName("ProductInvoiceNewId");
            this.Property(t => t.ProductInvoiceOldId).HasColumnName("ProductInvoiceOldId");
            this.Property(t => t.ProductInboundId).HasColumnName("ProductInboundId");
            this.Property(t => t.RemainingAmount).HasColumnName("RemainingAmount");

            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.CreatedUserName).HasColumnName("CreatedUserName");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");
            this.Property(t => t.AmountPayment).HasColumnName("AmountPayment");
            this.Property(t => t.AmountReceipt).HasColumnName("AmountReceipt");
            this.Property(t => t.WarehouseDestinationId).HasColumnName("WarehouseDestinationId");
        }
    }
}
