using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwLogVipMap : EntityTypeConfiguration<vwLogVip>
    {
        public vwLogVipMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Name).HasMaxLength(150);
            this.Property(t => t.Status).HasMaxLength(100);
            this.Property(t => t.Ratings).HasMaxLength(100);


            // Table & Column Mappings
            this.ToTable("vwSale_LogVip");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            //this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Ratings).HasColumnName("Ratings");
            this.Property(t => t.ApprovedUserId).HasColumnName("ApprovedUserId");
            this.Property(t => t.ApprovedDate).HasColumnName("ApprovedDate");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.is_approved).HasColumnName("is_approved");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.LoyaltyPointId).HasColumnName("LoyaltyPointId");
            this.Property(t => t.LoyaltyPointName).HasColumnName("LoyaltyPointName");
            this.Property(t => t.ApprovedUserName).HasColumnName("ApprovedUserName");
            this.Property(t => t.BranchId).HasColumnName("BranchId");

        }
    }
}
