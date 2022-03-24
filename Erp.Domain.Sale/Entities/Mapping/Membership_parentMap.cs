using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class Membership_parentMap : EntityTypeConfiguration<Membership_parent>
    {
        public Membership_parentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Name).HasMaxLength(150);
            this.Property(t => t.Status).HasMaxLength(50);
            this.Property(t => t.Code).HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Sale_Membership_parent");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.QRCode).HasColumnName("QRCode");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SerialNumber).HasColumnName("SerialNumber");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.isPrint).HasColumnName("isPrint");
            this.Property(t => t.NumberPrint).HasColumnName("NumberPrint");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.is_extend).HasColumnName("is_extend");
            this.Property(t => t.ExpiryDateOld).HasColumnName("ExpiryDateOld");
            this.Property(t => t.idcu).HasColumnName("idcu");

        }
    }
}
