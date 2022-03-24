using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class CommisionInvoice
    {
        public CommisionInvoice()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        public Nullable<int> CommissionCusId { get; set; }

        public string StartSymbol { get; set; }
        public decimal? StartAmount { get; set; }
        public string EndSymbol { get; set; }
        public decimal? EndAmount { get; set; }
        public string Type { get; set; }
        public decimal? CommissionValue { get; set; }
        public Nullable<bool> IsMoney { get; set; }
        public Nullable<bool> IsVIP { get; set; }
        public Nullable<int> SalesPercent { get; set; }
        public string Name { get; set; }
    }
}
