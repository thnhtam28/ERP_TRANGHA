using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities
{
    public class KH_TUONGTAC
    {
        public KH_TUONGTAC()
        {
        }
        public int KH_TUONGTAC_ID { get; set; }
        public int? BranchId { get; set; }
        public int? THANG { get; set; }
        public int? NAM { get; set; }
        public string NGAYLAP { get; set; }
        public int? NGUOILAP_ID { get; set; }
        public int? KHACHHANG_ID { get; set; }
        public string HINHTHUC_TUONGTAC { get; set; }
        public string GIO_TUONGTAC { get; set; }
        public string LOAI_TUONGTAC { get; set; }
        public string PHANLOAI_TUONGTAC { get; set; }
        public string TINHTRANG_TUONGTAC { get; set; }
        public string MUCDO_TUONGTAC { get; set; }
        public string GIAIPHAP_TUONGTAC { get; set; }
        public string MUCCANHBAO_TUONGTAC { get; set; }
        public string NGAYTUONGTAC_TIEP { get; set; }
        public string GIOTUONGTAC_TIEP { get; set; }
        public string HINH_ANH { get; set; }
        public string GHI_CHU { get; set; }
        public bool? IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string KETQUA_SAUTUONGTAC { get; set; }
    }
}
