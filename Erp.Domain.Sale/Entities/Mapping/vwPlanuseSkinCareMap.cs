using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwPlanuseSkinCareMap : EntityTypeConfiguration<vwPlanuseSkinCare>
    {
        public vwPlanuseSkinCareMap()
        {
            // Primary Key
            this.HasKey(t => t.TargetId);

            // Properties

            // Table & Column Mappings
            this.ToTable("vwPlanuseSkinCare");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.TargetId).HasColumnName("TargetId");
            this.Property(t => t.ProductInvoiceCode).HasColumnName("ProductInvoiceCode");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");

            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.ManagerName).HasColumnName("ManagerName");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.SOLUONG).HasColumnName("SOLUONG");
            this.Property(t => t.soluongdung).HasColumnName("soluongdung");
            this.Property(t => t.soluongtra).HasColumnName("soluongtra");
            this.Property(t => t.soluongchuyen).HasColumnName("soluongchuyen");
            this.Property(t => t.soluongconlai).HasColumnName("soluongconlai");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.GladLevel).HasColumnName("GladLevel");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.THANGTOANHET).HasColumnName("THANGTOANHET");
            //this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.ManagerStaffId).HasColumnName("ManagerStaffId");
        }
    }
}
