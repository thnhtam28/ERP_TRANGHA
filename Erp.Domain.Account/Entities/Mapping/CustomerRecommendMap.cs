using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities.Mapping
{
    public class CustomerRecommendMap : EntityTypeConfiguration<CustomerRecommend>
    {
        public CustomerRecommendMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            // Table & Column Mappings
            this.ToTable("Sale_CustomerRecommend");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
           
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            //this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomerId_new).HasColumnName("CustomerId_new");
            //this.Property(t => t.idcu).HasColumnName("idcu");

        }
    }
}
