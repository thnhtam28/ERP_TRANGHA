using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwMembership_parent
    {
        public vwMembership_parent()
        {
        }

        public Int64 Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }

        public int CustomerId { get; set; }
        public int ProductInvoiceId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string QRCode { get; set; }
        public string Note { get; set; }
        public string SerialNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public int Total { get; set; }
        public bool isPrint { get; set; }
        public Nullable<int> NumberPrint { get; set; }
        public int BranchId { get; set; }
        public Nullable<int> is_extend { get; set; }
        public Nullable<System.DateTime> ExpiryDateOld { get; set; }
        public string idcu { get; set; }

        public long ProductInvoiceDetailId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string ProductType { get; set; }

        public Nullable<int> solandadung { get; set; }
        public Nullable<int> ProductInvoiceId_Return { get; set; }

        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ManagerName { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string TargetCode { get; set; }

        public Nullable<int> ChiefUserId { get; set; }
        public string ChiefUserFullName { get; set; }

        public string UserTypeName_kd { get; set; }

        public decimal? tienconno { get; set; }

        

    }
}
