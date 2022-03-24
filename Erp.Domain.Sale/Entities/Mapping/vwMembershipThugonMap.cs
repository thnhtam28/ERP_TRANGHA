using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwMembershipThugonMap : EntityTypeConfiguration<vwMembershipThugon>
    {
        public vwMembershipThugonMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Name).HasMaxLength(150);
            //this.Property(t => t.Status).HasMaxLength(50);
            //this.Property(t => t.TargetModule).HasMaxLength(100);
            //this.Property(t => t.Code).HasMaxLength(100);
            //this.Property(t => t.TargetCode).HasMaxLength(100);


            // Table & Column Mappings
            this.ToTable("vwMembership_thugon_2");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");         
            //this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductInvoiceCode).HasColumnName("ProductInvoiceCode");
            //this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.TargetId).HasColumnName("TargetId");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");           
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.ManagerName).HasColumnName("ManagerName");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.SOLUONG).HasColumnName("SOLUONG");
            this.Property(t => t.soluongconlai).HasColumnName("soluongconlai");
            this.Property(t => t.soluongdung).HasColumnName("soluongdung");
            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.soluongtra).HasColumnName("soluongtra");
            this.Property(t => t.soluongchuyen).HasColumnName("soluongchuyen");

            
        }
    }
}
