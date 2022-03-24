using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class LogServiceRemminderMap : EntityTypeConfiguration<LogServiceRemminder>
    {
        public LogServiceRemminderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
          
                        this.Property(t => t.ReminderName).HasMaxLength(300);


            // Table & Column Mappings
            this.ToTable("Sale_LogServiceRemminder");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            //this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.ProductInvoiceDetailId).HasColumnName("ProductInvoiceDetailId");
            this.Property(t => t.ReminderId).HasColumnName("ReminderId");
            this.Property(t => t.ReminderName).HasColumnName("ReminderName");
            this.Property(t => t.ReminderDate).HasColumnName("ReminderDate");
            this.Property(t => t.ServiceId).HasColumnName("ServiceId");

        }
    }
}
