using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class BC_DOANHSO_NHANHANGMap : EntityTypeConfiguration<BC_DOANHSO_NHANHANG>
    {
        public BC_DOANHSO_NHANHANGMap()
        {
            // Primary Key
            this.HasKey(t => t.DOANHSO_NHANHANG_ID);

            ///

            this.ToTable("BC_DOANHSO_NHANHANG");
            this.Property(t => t.DOANHSO_NHANHANG_ID).HasColumnName("DOANHSO_NHANHANG_ID");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");


            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.THANG).HasColumnName("THANG");
            this.Property(t => t.NAM).HasColumnName("NAM");
            this.Property(t => t.TRANG_THAI).HasColumnName("TRANG_THAI");
            this.Property(t => t.NGAY_TAO).HasColumnName("NGAY_TAO");
            this.Property(t => t.MA_DONHANG).HasColumnName("MA_DONHANG");
            this.Property(t => t.TONG_TIEN).HasColumnName("TONG_TIEN");
            this.Property(t => t.TONG_THU).HasColumnName("TONG_THU");
            this.Property(t => t.DA_THANHTOAN).HasColumnName("DA_THANHTOAN");
            this.Property(t => t.CON_NO).HasColumnName("CON_NO");


            this.Property(t => t.TEN_KH).HasColumnName("TEN_KH");
            this.Property(t => t.MA_KH).HasColumnName("MA_KH");
            this.Property(t => t.KHACHANG_ID).HasColumnName("KHACHHANG_ID");
            this.Property(t => t.NHANVIEN_QLY).HasColumnName("NHANVIEN_QLY");

            this.Property(t => t.CHI_NHANH).HasColumnName("CHI_NHANH");
            this.Property(t => t.NGAY_KETTOAN).HasColumnName("NGAY_KETTOAN");
            this.Property(t => t.TRANGTHAI_GHISO).HasColumnName("TRANGTHAI_GHISO");
            this.Property(t => t.GIAMGIA_VIP).HasColumnName("GIAMGIA_VIP");
            this.Property(t => t.GIAMGIA_KM).HasColumnName("GIAMGIA_KM");

            this.Property(t => t.GIAMGIA_DB).HasColumnName("GIAMGIA_DB");
            this.Property(t => t.TINH_CHO).HasColumnName("TINH_CHO");
            this.Property(t => t.HANG_TANG).HasColumnName("HANG_TANG");
            this.Property(t => t.GD_DT).HasColumnName("GD_DT");
            this.Property(t => t.GD_NDT).HasColumnName("GD_NDT");


            this.Property(t => t.SP_HH).HasColumnName("SP_HH");
            this.Property(t => t.SP_DV).HasColumnName("SP_DV");
            this.Property(t => t.TYLE_HUONG).HasColumnName("TYLE_HUONG");
            this.Property(t => t.GHI_CHU).HasColumnName("GHI_CHU");

            this.Property(t => t.NHOM_QUANLY).HasColumnName("NHOM_QUANLY");
            this.Property(t => t.NHOM_QUANLY_ID).HasColumnName("NHOM_QUANLY_ID");

        }
    }
}
