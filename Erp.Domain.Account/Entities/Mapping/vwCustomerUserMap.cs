using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities.Mapping
{
    public class vwCustomerUserMap : EntityTypeConfiguration<vwCustomerUser>
    {
        public vwCustomerUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("vwSale_CustomerUser");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Image).HasColumnName("Image");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.FullName).HasColumnName("FullName");
        }

        
    }
}
