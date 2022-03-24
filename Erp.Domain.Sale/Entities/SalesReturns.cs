using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class SalesReturns
    {
        public SalesReturns()
        {

        }
        public int Id { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUserId { get; set; }

        public int? AssignedUserId { get; set; }

        public string Code { get; set; }

        public int? CustomerId { get; set; }

        public decimal? TotalAmount { get; set; }

        public double? TaxFee { get; set; }

        public string Status { get; set; }

        public string Note { get; set; }

        public int? BranchId { get; set; }

        public string PaymentMethod { get; set; }

        public decimal? PaidAmount { get; set; }

        public int? ProductInvoiceOldId { get; set; }

        public int? ProductInvoiceNewId { get; set; }

        public int? ProductInboundId { get; set; }

        public decimal? RemainingAmount { get; set; }
        public bool IsArchive { get; set; }
        public decimal? AmountPayment { get; set; }
        public decimal? AmountReceipt { get; set; }
        public Nullable<int> WarehouseDestinationId { get; set; }
    }
}
