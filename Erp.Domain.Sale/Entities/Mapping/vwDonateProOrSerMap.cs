using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwDonateProOrSerMap : EntityTypeConfiguration<vwDonateProOrSer>
    {
        public vwDonateProOrSerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
                        this.Property(t => t.TargetModule).HasMaxLength(150);


            // Table & Column Mappings
            this.ToTable("vwSale_DonateProOrSer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");

            this.Property(t => t.TargetId).HasColumnName("TargetId");
            this.Property(t => t.TargetModule).HasColumnName("TargetModule");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.ExpriryMonth).HasColumnName("ExpriryMonth");
            this.Property(t => t.TotalQuantity).HasColumnName("TotalQuantity");
            this.Property(t => t.RemainQuantity).HasColumnName("RemainQuantity");
            //
            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.ProductType).HasColumnName("ProductType");


        }
    }
}
