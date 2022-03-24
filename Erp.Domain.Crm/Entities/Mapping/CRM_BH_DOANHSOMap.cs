using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    public class CRM_BH_DOANHSOMap : EntityTypeConfiguration<CRM_BH_DOANHSO>
    {
        public CRM_BH_DOANHSOMap()
        {
            //primary key
            this.HasKey(t => t.KH_BANHANG_DOANHSO_ID);
            //TABLE colum % maping
            this.ToTable("CRM_KH_BANHANG_DOANHSO");
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
        }

    }
}
