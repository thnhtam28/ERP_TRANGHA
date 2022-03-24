using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class MoneyMove
    {
        public MoneyMove()
        {

        }
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public int? AssignedUserId { get; set; }
        public int? FromProductInvoiceId { get; set; }
        public int? ToProductInvoiceId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Note { get; set; }
        public bool? IsArchive { get; set; }
        public string idcu { get; set; }
        public int? BranchId { get; set; }


    }
}
