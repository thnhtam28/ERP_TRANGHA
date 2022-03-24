using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class Membership_parent
    {
        public Membership_parent()
        {
        }

        public Int64 Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        //public string Name { get; set; }

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

    }
}
