using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class ProductInvoice
    {
        public ProductInvoice()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string Code { get; set; }
        public decimal TotalAmount { get; set; }
        public double TaxFee { get; set; }
        public double Discount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string ShipName { get; set; }
        public string ShipPhone { get; set; }
        public string ShipAddress { get; set; }
        public string ShipWardId { get; set; }
        public string ShipDistrictId { get; set; }
        public string ShipCityId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public bool IsArchive { get; set; }
        public int BranchId { get; set; }
        public string PaymentMethod { get; set; }
        public string CancelReason { get; set; }
        public string CodeInvoiceRed { get; set; }
        public bool IsReturn { get; set; }
        public Nullable<System.DateTime> NextPaymentDate { get; set; }
        public string Type { get; set; }

        public Nullable<bool> IsBonusSales { get; set; }
        public decimal? DoanhThu { get; set; }
        public double? Discount_VIP { get; set; }
        public double? Discount_KM { get; set; }
        public double? Discount_DB { get; set; }
        public string CountForBrand { get; set; }
        public int? ProductInvoiceId_Return { get; set; }

    }
}
