﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities
{
    public class vwKH_GIOITHIEU
    {
        public vwKH_GIOITHIEU()
        {

        }
        public int KH_GIOITHIEU_ID { get; set; }
        public int? BranchId { get; set; }
        public int? KHACHHANG_ID { get; set; }
        public string LOAI_GIOITHIEU { get; set; }
        public string TRANGTHAI_GIOITHIEU { get; set; }
        public string NOIDUNG { get; set; }
        public int? TYLE_THANHCONG { get; set; }
        public bool? IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string BranchName { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string CustomerCode { get; set; }
        public string ModifiedUserName { get; set; }
        public string FullName { get; set; }
        public int? UserId { get; set; }
        public int? ManagerStaffId { get; set; }
        public string ManagerUserName { get; set; }
    }
}
