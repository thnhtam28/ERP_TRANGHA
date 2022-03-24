using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class LogPromotionMap : EntityTypeConfiguration<LogPromotion>
    {
        public LogPromotionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(350);
                        this.Property(t => t.ProductInvoiceCode).HasMaxLength(100);
            this.Property(t => t.ProductId).HasMaxLength(150);
            this.Property(t => t.CommissionCusCode).HasMaxLength(100);
            this.Property(t => t.TargetModule).HasMaxLength(150);
            this.Property(t => t.Type).HasMaxLength(100);
            this.Property(t => t.Code).HasMaxLength(100);


            // Table & Column Mappings
            this.ToTable("Sale_LogPromotion");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.ProductInvoiceCode).HasColumnName("ProductInvoiceCode");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.CommissionCusCode).HasColumnName("CommissionCusCode");
            this.Property(t => t.TargetID).HasColumnName("TargetID");
            this.Property(t => t.TargetModule).HasColumnName("TargetModule");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CommissionValue).HasColumnName("CommissionValue");
            this.Property(t => t.IsMoney).HasColumnName("IsMoney");
            this.Property(t => t.GiftProductId).HasColumnName("GiftProductId");
            this.Property(t => t.DonateProOrSerId).HasColumnName("DonateProOrSerId");

            this.Property(t => t.ProductQuantity).HasColumnName("ProductQuantity");
            this.Property(t => t.ProductSymbolQuantity).HasColumnName("ProductSymbolQuantity");
            this.Property(t => t.StartSymbol).HasColumnName("StartSymbol");
            this.Property(t => t.EndSymbol).HasColumnName("EndSymbol");
            this.Property(t => t.MaChungTuLienQuan).HasColumnName("MaChungTuLienQuan");
            this.Property(t => t.StartAmount).HasColumnName("StartAmount");
            this.Property(t => t.EndAmount).HasColumnName("EndAmount");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.CommissionCusId).HasColumnName("CommissionCusId");
        }
    }
}
