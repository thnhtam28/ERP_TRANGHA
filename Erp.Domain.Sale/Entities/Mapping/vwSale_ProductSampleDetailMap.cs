using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwSale_ProductSampleDetailMap : EntityTypeConfiguration<vwProductSampleDetail>
    {
        public vwSale_ProductSampleDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            
                        this.Property(t => t.Status).HasMaxLength(100);
            this.Property(t => t.Note).HasMaxLength(250);
            this.Property(t => t.FirstName).HasMaxLength(100);
            this.Property(t => t.LastName).HasMaxLength(100);
            this.Property(t => t.CustomerCode).HasMaxLength(20);
            this.Property(t => t.ProductSampleCode).HasMaxLength(100);
            this.Property(t => t.ProductName).HasMaxLength(150);
            this.Property(t => t.ProductCode).HasMaxLength(30);
            this.Property(t => t.CustomerImage).HasMaxLength(150);
            this.Property(t => t.ProductImage).HasMaxLength(150);


            // Table & Column Mappings
            this.ToTable("vwSale_ProductSampleDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
           

            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.ProductSampleId).HasColumnName("ProductSampleId");
            this.Property(t => t.ProductSampleCode).HasColumnName("ProductSampleCode");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.CustomerImage).HasColumnName("CustomerImage");
            this.Property(t => t.ProductImage).HasColumnName("ProductImage");
            this.Property(t => t.ProductLinkId).HasColumnName("ProductLinkId");

        }
    }
}
