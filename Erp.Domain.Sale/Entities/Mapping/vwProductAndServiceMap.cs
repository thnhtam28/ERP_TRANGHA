using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwProductAndServiceMap : EntityTypeConfiguration<vwProductAndService>
    {
        public vwProductAndServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(150);
            this.Property(t => t.Code).HasMaxLength(30);
            this.Property(t => t.Description).HasMaxLength(2000);
            this.Property(t => t.Unit).HasMaxLength(50);
            this.Property(t => t.Type).HasMaxLength(20);
            this.Property(t => t.CategoryCode).HasMaxLength(20);


            // Table & Column Mappings
            this.ToTable("vwSale_ProductAndService");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.PriceInbound).HasColumnName("PriceInbound");
            this.Property(t => t.PriceOutbound).HasColumnName("PriceOutbound");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode");
            this.Property(t => t.CategoryName).HasColumnName("CategoryName");

            this.Property(t => t.MinInventory).HasColumnName("MinInventory");
            this.Property(t => t.Barcode).HasColumnName("Barcode");
            this.Property(t => t.Image_Name).HasColumnName("Image_Name");
            this.Property(t => t.ProductGroup).HasColumnName("ProductGroup");
            this.Property(t => t.Manufacturer).HasColumnName("Manufacturer");
            this.Property(t => t.Size).HasColumnName("Size");
            this.Property(t => t.IsCombo).HasColumnName("IsCombo");
            this.Property(t => t.QuantityTotalInventory).HasColumnName("QuantityTotalInventory");
            this.Property(t => t.DiscountStaff).HasColumnName("DiscountStaff");
            this.Property(t => t.IsMoneyDiscount).HasColumnName("IsMoneyDiscount");
            this.Property(t => t.ProductLinkId).HasColumnName("ProductLinkId");
            this.Property(t => t.ProductLinkName).HasColumnName("ProductLinkName");
            this.Property(t => t.ProductLinkCode).HasColumnName("ProductLinkCode");
     		this.Property(t => t.QuantityDayUsed).HasColumnName("QuantityDayUsed");
            this.Property(t => t.QuantityDayNotify).HasColumnName("QuantityDayNotify");
            this.Property(t => t.Origin).HasColumnName("Origin");
            this.Property(t => t.EquimentGroup).HasColumnName("EquimentGroup");
        }
    }
}
