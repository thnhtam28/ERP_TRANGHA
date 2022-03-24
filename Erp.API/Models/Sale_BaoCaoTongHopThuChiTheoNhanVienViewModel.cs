
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoTongHopThuChiTheoNhanVienViewModel
    {
        public DateTime? CreatedDate { get; set; }
        public decimal? ThuBH { get; set; }
        public decimal? ThuCongNo { get; set; }
        public decimal? ThuTraNCC { get; set; }
        public decimal? ThuKhac { get; set; }
        public decimal? XuatNCC { get; set; }
        public decimal? XuatHBTL { get; set; }
        public decimal? TongThu { get; set; }
        public decimal? MuaHang { get; set; }
        public decimal? ChiCongNo { get; set; }
        public decimal? ChiHBTL { get; set; }
        public decimal? ChiKhac { get; set; }
        public decimal? HangTra { get; set; }
        public decimal? TongChi { get; set; }
        public decimal? GiaoNop { get; set; }

    }
}