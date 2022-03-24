using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    public class CRM_KH_BANHANG_CTIETMap : EntityTypeConfiguration<CRM_KH_BANHANG_CTIET>
    {
        public CRM_KH_BANHANG_CTIETMap()
        {
            // Primary Key
            this.HasKey(t => t.KH_BANHANG_CTIET_ID);

            // Properties
            //this.Property(t => t.GHI_CHU).HasMaxLength(50);
            //this.Property(t => t.CountForBrand).HasMaxLength(500);


            // Table & Column Mappings
            this.ToTable("CRM_KH_BANHANG_CTIET");
            this.Property(t => t.KH_BANHANG_CTIET_ID).HasColumnName("KH_BANHANG_CTIET_ID");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.KH_BANHANG_ID).HasColumnName("KH_BANHANG_ID");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.THANG).HasColumnName("THANG");
            this.Property(t => t.NAM).HasColumnName("NAM");
            this.Property(t => t.KHACHHANG_ID).HasColumnName("KHACHHANG_ID");
            this.Property(t => t.NOIDUNG).HasColumnName("NOIDUNG");
            this.Property(t => t.TYLE_THANHCONG).HasColumnName("TYLE_THANHCONG");
            this.Property(t => t.TYLE_THANHCONG_REVIEW).HasColumnName("TYLE_THANHCONG_REVIEW");

            this.Property(t => t.GHI_CHU).HasColumnName("GHI_CHU");
            //this.Property(t => t.CompanyName).HasColumnName("GHI_CHU");

        }
    }
}
