using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class Sale_TARGET_NVKDMap  :EntityTypeConfiguration<Sale_TARGET_NVKD>
    {
        public Sale_TARGET_NVKDMap()
        {
            // Primary Key
            
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("Sale_TARGET_NVKD");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.month).HasColumnName("month");
            this.Property(t => t.year).HasColumnName("year");
            this.Property(t => t.OldOrlane).HasColumnName("OldOrlane");
            this.Property(t => t.NewOrlane).HasColumnName("NewOrlane");
            this.Property(t => t.Annayake).HasColumnName("Annayake");
            this.Property(t => t.LennorGreyl).HasColumnName("LennorGreyl");
        }
    }
}
