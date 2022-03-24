using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities
{
    public class vwCRM_KH_BANHANG_CTIET
    {
        public vwCRM_KH_BANHANG_CTIET()
        {

        }
        public int KH_BANHANG_CTIET_ID { get; set; }
        public int BranchId { get; set; }
        public int KH_BANHANG_ID { get; set; }

        public int? THANG { get; set; }
        public int? NAM { get; set; }
        public int KHACHHANG_ID { get; set; }
        public string NOIDUNG { get; set; }

        public int? TYLE_THANHCONG { get; set; }

        public int? TYLE_THANHCONG_REVIEW { get; set; }
        public string GHI_CHU { get; set; }

        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string CompanyName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
        public string CRM_Sale_ProductInvoiceCode { get; set; }
        public string CountForBrand { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? ProductInvoiceId { get; set; }
        public int? NGUOILAP_ID { get; set; }
    }
}
