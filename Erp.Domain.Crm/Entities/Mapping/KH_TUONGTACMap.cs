using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    public class KH_TUONGTACMap : EntityTypeConfiguration<KH_TUONGTAC>
    {
        public KH_TUONGTACMap()
        {
            // Primary Key
            this.HasKey(t => t.KH_TUONGTAC_ID);

            // Properties

            // Table & Column Mappings
            this.ToTable("CRM_KH_TUONGTAC");
            this.Property(t => t.KH_TUONGTAC_ID).HasColumnName("KH_TUONGTAC_ID");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.THANG).HasColumnName("THANG");
            this.Property(t => t.NAM).HasColumnName("NAM");
            this.Property(t => t.NGAYLAP).HasColumnName("NGAYLAP");
            this.Property(t => t.NGUOILAP_ID).HasColumnName("NGUOILAP_ID");
            this.Property(t => t.KHACHHANG_ID).HasColumnName("KHACHHANG_ID");
            this.Property(t => t.HINHTHUC_TUONGTAC).HasColumnName("HINHTHUC_TUONGTAC");
            this.Property(t => t.GIO_TUONGTAC).HasColumnName("GIO_TUONGTAC");
            this.Property(t => t.LOAI_TUONGTAC).HasColumnName("LOAI_TUONGTAC");
            this.Property(t => t.PHANLOAI_TUONGTAC).HasColumnName("PHANLOAI_TUONGTAC");
            this.Property(t => t.TINHTRANG_TUONGTAC).HasColumnName("TINHTRANG_TUONGTAC");
            this.Property(t => t.MUCDO_TUONGTAC).HasColumnName("MUCDO_TUONGTAC");
            this.Property(t => t.GIAIPHAP_TUONGTAC).HasColumnName("GIAIPHAP_TUONGTAC");
            this.Property(t => t.MUCCANHBAO_TUONGTAC).HasColumnName("MUCCANHBAO_TUONGTAC");
            this.Property(t => t.NGAYTUONGTAC_TIEP).HasColumnName("NGAYTUONGTAC_TIEP");
            this.Property(t => t.GIOTUONGTAC_TIEP).HasColumnName("GIOTUONGTAC_TIEP");
            this.Property(t => t.HINH_ANH).HasColumnName("HINH_ANH");
            this.Property(t => t.GHI_CHU).HasColumnName("GHI_CHU");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.KETQUA_SAUTUONGTAC).HasColumnName("KETQUA_SAUTUONGTAC");

        }
    }
}
