using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities.Mapping
{
    public class PaymentMap : EntityTypeConfiguration<Payment>
    {
        public PaymentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(150);
            this.Property(t => t.Code).HasMaxLength(20);
            this.Property(t => t.PaymentMethod).HasMaxLength(150);
            this.Property(t => t.BankAccountNo).HasMaxLength(20);
            this.Property(t => t.BankAccountName).HasMaxLength(150);
            this.Property(t => t.BankName).HasMaxLength(150);
            this.Property(t => t.Receiver).HasMaxLength(150);


            // Table & Column Mappings
            this.ToTable("Account_Payment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.PaymentMethod).HasColumnName("PaymentMethod");
            this.Property(t => t.BankAccountNo).HasColumnName("BankAccountNo");
            this.Property(t => t.BankAccountName).HasColumnName("BankAccountName");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.Receiver).HasColumnName("Receiver");
            this.Property(t => t.SalerId).HasColumnName("SalerId");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.VoucherDate).HasColumnName("VoucherDate");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.TargetId).HasColumnName("TargetId");
            this.Property(t => t.TargetName).HasColumnName("TargetName");
            this.Property(t => t.MaChungTuGoc).HasColumnName("MaChungTuGoc");
            this.Property(t => t.LoaiChungTuGoc).HasColumnName("LoaiChungTuGoc");
            this.Property(t => t.CancelReason).HasColumnName("CancelReason");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");
        }
    }
}
