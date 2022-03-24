using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities
{
    public class vwCRM_TK_TUONGTAC
    {
        public vwCRM_TK_TUONGTAC()
        {

        }
        public Int64 Id { get; set; }
        public int? BranchId { get; set; }

        public string NGAYLAP { get; set; }

        public string NGUOI_LAP { get; set; }

        public int? NGUOILAP_ID { get; set; }
        public int? TONG_QL { get; set; }
        public int? TONG_PLAN { get; set; }
        public int? SOTUONGTAC { get; set; }
        public int? SO_QUAHAN { get; set; }
        public int? CHUA_PLAN { get; set; }
        public int? CHUA_PLAN_NEXT { get; set; }
    }
}
