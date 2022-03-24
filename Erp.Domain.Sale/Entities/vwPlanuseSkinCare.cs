using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwPlanuseSkinCare
    {
        public vwPlanuseSkinCare()
        { }
        public int? CustomerId { get; set; }
        public int? BranchId { get; set; }
        public int? TargetId { get; set; }
        public string ProductInvoiceCode { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public string ManagerName { get; set; }
        public string Type { get; set; }
        public int? CreatedUserId { get; set; }
        public int? SOLUONG { get; set; }
        public int? soluongdung { get; set; }
        public int? soluongtra { get; set; }
        public int? soluongchuyen { get; set; }
        public int? soluongconlai { get; set; }
        public string Phone { get; set; }
        public string GladLevel { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string THANGTOANHET { get; set; }
        //public decimal? Amount { get; set; }
        public int? ProductInvoiceId { get; set; }
        public int? ManagerStaffId { get; set; }
    }
}
