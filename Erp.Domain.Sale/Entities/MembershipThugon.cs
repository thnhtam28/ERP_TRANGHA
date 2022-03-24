using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class MembershipThugon
    {
        public MembershipThugon()
        {

        }

        public Int64 Id { get; set; }
        
        public int? CustomerId { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }      
        //public string Name { get; set; }

       
        public string ProductName { get; set; }       
        public Int64? TargetId { get; set; }             
        public string Type { get; set; }
        public int? BranchId { get; set; }
        public string ProductInvoiceCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public int? Quantity { get; set; }
        public string ManagerName { get; set; }
        public int? SOLUONG { get; set; }
        public int? ProductInvoiceId { get; set; }
        public int? soluongconlai { get; set; }
        public int? soluongdung { get; set; }
    }
}
