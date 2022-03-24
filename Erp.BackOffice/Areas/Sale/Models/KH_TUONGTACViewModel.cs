﻿using Erp.BackOffice.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Erp.Domain.Entities;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class KH_TUONGTACViewModel
    {
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
        [Display(Name = "CustomerName", ResourceType = typeof(Wording))]
        public string CustomerName { get; set; }
        [Display(Name = "CustomerCode", ResourceType = typeof(Wording))]
        public string CustomerCode { get; set; }
        [Display(Name = "Phone", ResourceType = typeof(Wording))]
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string LICHTUONGTATIEP { get; set; }
        public string Code { get; set; }
 
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Wording))]
        public string Description { get; set; }
        public string NGAYKH { get; set; }
        public int? CustomerId { get; set; }
        public string KETQUA_SAUTUONGTAC { get; set; }
        public int? is_checkedKH_TUONGTAC { get; set; }
        public int? is_checked { get; set; }
        public List<KH_TUONGTACViewModel> KH_TUONGTAC { get; set; }


        //doi ngày dạng datetime
         public DateTime? Ngay_Lap { get; set; }

        public string NGUOI_LAP { get; set; }
      
        public int? TONG_QL { get; set; }
        public int? TONG_PLAN { get; set; }
        public int? SOTUONGTAC { get; set; }
        public int? SO_QUAHAN { get; set; }
        public int? CHUA_PLAN { get; set; }
        public int? CHUA_PLAN_NEXT { get; set; }
        public int? ManagerStaffId { get; set; }
        public string ManagerStaffName { get; set; }
    }
}