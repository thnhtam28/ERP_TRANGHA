using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    public class vwKH_SAPHETSPMap : EntityTypeConfiguration<vwKH_SAPHETSP>
    {
        public vwKH_SAPHETSPMap()
        {
            //primary key
            this.HasKey(t => t.Id);
            //TABLE colum % maping
            this.ToTable("vwKH_SAPHETSP");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.ProductInvoiceCode).HasColumnName("ProductInvoiceCode");
            this.Property(t => t.ProductInvoiceStatus).HasColumnName("ProductInvoiceStatus");
            this.Property(t => t.Ngay_xuatkho).HasColumnName("Ngay_xuatkho");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.QuantityDayUsed).HasColumnName("QuantityDayUsed");
            this.Property(t => t.SPSAPHETHAN).HasColumnName("SPSAPHETHAN");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductOutboundType).HasColumnName("ProductOutboundType");
            this.Property(t => t.Phone).HasColumnName("Phone");
        }
    }
}
