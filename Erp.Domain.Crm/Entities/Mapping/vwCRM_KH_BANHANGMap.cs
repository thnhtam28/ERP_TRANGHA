using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    class vwCRM_KH_BANHANGMap : EntityTypeConfiguration<vwCRM_KH_BANHANG>
    {

        public vwCRM_KH_BANHANGMap()
        {
            // Primary Key
            this.HasKey(t => t.KH_BANHANG_ID);

            // Properties
            //this.Property(t => t.CustomerCode).HasMaxLength(20);
            //this.Property(t => t.CustomerName).HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("vwCRM_KH_BANHANG");
            this.Property(t => t.KH_BANHANG_ID).HasColumnName("KH_BANHANG_ID");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.THANG).HasColumnName("THANG");
            this.Property(t => t.NAM).HasColumnName("NAM");
            this.Property(t => t.NGUOILAP_ID).HasColumnName("NGUOILAP_ID");
            this.Property(t => t.CountForBrand).HasColumnName("CountForBrand");
            this.Property(t => t.TARGET_BRAND).HasColumnName("TARGET_BRAND");
            this.Property(t => t.GHI_CHU).HasColumnName("GHI_CHU");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.FullName).HasColumnName("FullName");
            
        }
    }
}
