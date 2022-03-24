using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities
{
    public class vwCustomer_ProductInvoice
    {
        public vwCustomer_ProductInvoice()
        {

        }
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Code { get; set; }
        public int? ManagerStaffId { get; set; }
        public int? ProductInvoiceId { get; set; }
        public string ProductInvoiceCode { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

      public string Phone { get; set; }

    }
}
