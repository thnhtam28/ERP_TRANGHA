using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class Sale_TARGET_NVKD
    {
        public int Id { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public int BranchId { get; set; }
        public decimal? OldOrlane { get; set; }
        public decimal? NewOrlane { get; set; }
        public decimal? Annayake { get; set; }
        public decimal? LennorGreyl { get; set; }

        public Nullable<bool> IsDeleted { get; set; }
    }
}
