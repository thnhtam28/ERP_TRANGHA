using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class LogPromotion
    {
        public LogPromotion()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        public string Name { get; set; }

        public string ProductInvoiceCode { get; set; }
        public string ProductId { get; set; }
        public string CommissionCusCode { get; set; }
        public Nullable<int> TargetID { get; set; }
        public string TargetModule { get; set; }
        public string Type { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string Code { get; set; }
        public decimal? CommissionValue { get; set; }
        public Nullable<bool> IsMoney { get; set; }
        public Nullable<int> GiftProductId { get; set; }
        public Nullable<int> DonateProOrSerId { get; set; }
        public Nullable<int> ProductQuantity { get; set; }
        public string ProductSymbolQuantity { get; set; }
        public string StartSymbol { get; set; }
        public string EndSymbol { get; set; }
        public string MaChungTuLienQuan { get; set; }
        public decimal? EndAmount { get; set; }
        public decimal? StartAmount { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }

        public int? CommissionCusId { get; set; }
    }
}
