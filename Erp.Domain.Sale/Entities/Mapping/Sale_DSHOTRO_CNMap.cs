using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class Sale_DSHOTRO_CNMap : EntityTypeConfiguration<Sale_DSHOTRO_CN>
    {
        public Sale_DSHOTRO_CNMap()
        {
            // Primary Key

            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("Sale_DSHOTRO_CN");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.OldOrlane).HasColumnName("OldOrlane");
            this.Property(t => t.NewOrlane).HasColumnName("NewOrlane");
            this.Property(t => t.Annayake).HasColumnName("Annayake");
            this.Property(t => t.LennorGreyl).HasColumnName("LennorGreyl");
        }
    }
    
}
