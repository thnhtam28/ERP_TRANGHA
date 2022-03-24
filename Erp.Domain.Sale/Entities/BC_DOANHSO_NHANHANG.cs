using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class BC_DOANHSO_NHANHANG
    {
        public BC_DOANHSO_NHANHANG()
        {

        }
        public int DOANHSO_NHANHANG_ID { get; set; }
        
        public int BranchId { get; set; }
        public int? THANG { get; set; }
        public int? NAM { get; set; }
        public string TRANG_THAI { get; set; }
        public string NGAY_TAO { get; set; }
        public string MA_DONHANG { get; set; }
        public decimal TONG_TIEN { get; set; }
        public decimal? TONG_THU { get; set; }
        public decimal? DA_THANHTOAN { get; set; }
        public decimal? CON_NO { get; set; }
        public string TEN_KH { get; set; }
        public string MA_KH { get; set; }
        public Nullable<int> KHACHANG_ID { get; set; }
        public string NHANVIEN_QLY { get; set; }
        public string CHI_NHANH { get; set; }
        public string NGAY_KETTOAN { get; set; }
        public string TRANGTHAI_GHISO { get; set; }
        public double? GIAMGIA_VIP { get; set; }
        public double? GIAMGIA_KM { get; set; }
        public double? GIAMGIA_DB { get; set; }
        public string TINH_CHO { get; set; }
        public string HANG_TANG { get; set; }
        public string GD_DT { get; set; }
        public string GD_NDT { get; set; }
        public string SP_HH { get; set; }
        public string SP_DV { get; set; }
        public double? TYLE_HUONG { get; set; }
        public string GHI_CHU { get; set; }
        public string NHOM_QUANLY { get; set; }
        public int? NHOM_QUANLY_ID { get; set; }

        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }
}
