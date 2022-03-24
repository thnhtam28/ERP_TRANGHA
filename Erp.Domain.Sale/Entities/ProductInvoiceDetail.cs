using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class ProductInvoiceDetail
    {
        public ProductInvoiceDetail()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<int> ProductInvoiceId { get; set; }
        public int ProductId { get; set; }
        public decimal? Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Unit { get; set; }

        public string LoCode { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string Note { get; set; }
        public int? is_TANG { get; set; }
        public int? SOLAN_TANG_DV { get; set; }

        public int? BranchId { get; set; }

    }
}
