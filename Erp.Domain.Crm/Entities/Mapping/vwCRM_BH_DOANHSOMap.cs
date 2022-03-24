using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace Erp.Domain.Crm.Entities.Mapping
{
    class vwCRM_BH_DOANHSOMap :EntityTypeConfiguration<vwCRM_BH_DOANHSO>
    {
        public vwCRM_BH_DOANHSOMap()
        {
            this.HasKey(t => t.KH_BANHANG_DOANHSO_ID);
              this.ToTable("vwCRM_KH_BANHANG_DOANHSO");
              this.Property(t => t.KH_BANHANG_DOANHSO_ID).HasColumnName("KH_BANHANG_DOANHSO_ID");
              this.Property(t => t.BranchId).HasColumnName("BranchId");
              this.Property(t => t.THANG).HasColumnName("THANG");
              this.Property(t => t.NAM).HasColumnName("NAM");
              this.Property(t => t.NGUOILAP_ID).HasColumnName("NGUOILAP_ID");
              this.Property(t => t.CountForBrand).HasColumnName("CountForBrand");
              this.Property(t => t.TARGET_BRAND).HasColumnName("TARGET_BRAND");
              this.Property(t => t.TARGET_DALAP).HasColumnName("TARGET_DALAP");
              this.Property(t => t.GHI_CHU).HasColumnName("GHI_CHU");
              this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
              this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
              this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
              this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
              this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
              this.Property(t => t.BranchName).HasColumnName("BranchName");
              this.Property(t => t.UserName).HasColumnName("UserName");
        }
    }
}
