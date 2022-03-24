using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    class vwCustomer_ProductInvoiceMap : EntityTypeConfiguration<vwCustomer_ProductInvoice>
    {

        public vwCustomer_ProductInvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.CustomerCode).HasMaxLength(20);
            //this.Property(t => t.CustomerName).HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("vwCustomer_ProductInvoice");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.ManagerStaffId).HasColumnName("ManagerStaffId");
            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.ProductInvoiceCode).HasColumnName("ProductInvoiceCode");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

            this.Property(t => t.Code).HasColumnName("Code");

        }
    }
}
