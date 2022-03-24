using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities
{
    public class vwKH_SAPHETSP
    {
        public vwKH_SAPHETSP()
        {
        }
         public int Id { get; set; }
         public int? CustomerId { get; set; }
         public string CustomerCode { get; set; }
         public string CompanyName { get; set; }
         public int? ProductInvoiceId { get; set; }
         public string ProductInvoiceCode { get; set; }
         public string ProductInvoiceStatus { get; set; }
         public string Ngay_xuatkho { get; set; }
         public int? Quantity { get; set; }
         public int? QuantityDayUsed { get; set; }
         public DateTime?  SPSAPHETHAN { get; set; }
         public string ProductCode { get; set; }
         public string ProductName { get; set; }
         public string ProductOutboundType { get; set; }
        public string Phone { get; set; }
    }
}
