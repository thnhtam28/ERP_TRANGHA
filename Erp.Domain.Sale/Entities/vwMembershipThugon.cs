using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwMembershipThugon
    {
        public vwMembershipThugon()
        {

        }
        public Int64? Id { get; set; }
        public int? TargetId { get; set; }
        public string CustomerId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        //public string Name { get; set; }

       
        public string ProductName { get; set; }
       
        public string Type { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string ProductInvoiceCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string ManagerName { get; set; }
        public Nullable<int> SOLUONG { get; set; }
        public Nullable<int> soluongdung { get; set; }
        public Nullable<int> soluongconlai { get; set; }
        public Nullable<int> ProductInvoiceId { get; set; }
        public Nullable<int> soluongtra { get; set; }
        public Nullable<int> soluongchuyen { get; set; }
    }
}
