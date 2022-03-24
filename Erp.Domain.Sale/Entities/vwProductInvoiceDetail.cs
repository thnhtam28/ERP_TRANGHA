using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwProductInvoiceDetail
    {
        public vwProductInvoiceDetail()
        {

        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<int> ProductInvoiceId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public decimal? Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Unit { get; set; }
        public string ProductType { get; set; }

        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductGroup { get; set; }
        public string ProductInvoiceCode { get; set; }

        public bool IsArchive { get; set; }
        public System.DateTime ProductInvoiceDate { get; set; }
        public decimal? Amount { get; set; }
        public string Manufacturer { get; set; }
        public int BranchId { get; set; }
        public Nullable<bool> IsCombo { get; set; }
        public string BranchName { get; set; }
        public string Type { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public int? CustomerId { get; set; }

        public string LoCode { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string DistrictId { get; set; }
        public string CityId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Year { get; set; }
        public string ProductImage { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string Origin { get; set; }
        public int? ManagerStaffId { get; set; }
        public string Status { get; set; }
        //public string ProductOutboundCode { get; set; }
        //public int? ProductOutboundId { get; set; }
        public string Note { get; set; }
        public int? is_TANG { get; set; }
        public int? SOLAN_TANG_DV { get; set; }
        public string CountForBrand { get; set; }
        public int? SOHOADON { get; set; }
        public decimal? Tiensaugiam { get; set; }
    }
}
