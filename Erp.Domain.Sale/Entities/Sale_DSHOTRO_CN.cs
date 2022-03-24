using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class Sale_DSHOTRO_CN
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int BranchId { get; set; }
        public decimal? OldOrlane { get; set; }
        public decimal? NewOrlane { get; set; }
        public decimal? Annayake { get; set; }
        public decimal? LennorGreyl { get; set; }

        public Nullable<bool> IsDeleted { get; set; }
    }
}
