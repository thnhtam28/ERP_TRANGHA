using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities
{
    public class CRM_KH_BANHANG
    {
        public CRM_KH_BANHANG()
        {

        }
        public int KH_BANHANG_ID { get;set;}
        public int? BranchId { get; set; }

        public int? THANG { get; set; }
        public int? NAM { get; set; }
        public int? NGUOILAP_ID { get; set; }
        public string CountForBrand { get; set; }

        public decimal? TARGET_BRAND { get; set; }

        public string GHI_CHU { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }
}
