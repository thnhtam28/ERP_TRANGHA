using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities.Mapping
{
    public class vwCustomerRecommendMap : EntityTypeConfiguration<vwCustomerRecommend>
    {
        public vwCustomerRecommendMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Name).HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("vwSale_CustomerRecommend");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
           

            this.Property(t => t.StartDate).HasColumnName("StartDate");
            //this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.CustomerId_new).HasColumnName("CustomerId_new");

        }


    }
}
