using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class MoneyMoveMap : EntityTypeConfiguration<MoneyMove>
    {
        public MoneyMoveMap()
        {
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Note).HasMaxLength(500);


            // Table & Column Mappings
            this.ToTable("Sale_MoneyMove");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.FromProductInvoiceId).HasColumnName("FromProductInvoiceId");
            this.Property(t => t.ToProductInvoiceId).HasColumnName("ToProductInvoiceId");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");
            this.Property(t => t.idcu).HasColumnName("idcu");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
        }
    }
}
