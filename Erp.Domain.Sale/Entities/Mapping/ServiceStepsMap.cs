using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class ServiceStepsMap : EntityTypeConfiguration<ServiceSteps>
    {
        public ServiceStepsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(350);
            

            // Table & Column Mappings
            this.ToTable("Sale_ServiceSteps");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.TotalMinute).HasColumnName("TotalMinute");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProductId).HasColumnName("ProductId");

        }
    }
}
