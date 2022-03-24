using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class CommisionInvoiceMap : EntityTypeConfiguration<CommisionInvoice>
    {
        public CommisionInvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
                        this.Property(t => t.StartSymbol).HasMaxLength(50);
            this.Property(t => t.EndSymbol).HasMaxLength(50);
            this.Property(t => t.Type).HasMaxLength(50);


            // Table & Column Mappings
            this.ToTable("Sale_CommisionInvoice");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.CommissionCusId).HasColumnName("CommissionCusId");

            this.Property(t => t.StartSymbol).HasColumnName("StartSymbol");
            this.Property(t => t.StartAmount).HasColumnName("StartAmount");
            this.Property(t => t.EndSymbol).HasColumnName("EndSymbol");
            this.Property(t => t.EndAmount).HasColumnName("EndAmount");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CommissionValue).HasColumnName("CommissionValue");
            this.Property(t => t.IsMoney).HasColumnName("IsMoney");
            this.Property(t => t.IsVIP).HasColumnName("IsVIP");
            this.Property(t => t.SalesPercent).HasColumnName("SalesPercent");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
