using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwProductSampleDetail
    {
        public vwProductSampleDetail()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        

        public Nullable<int> CustomerId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerCode { get; set; }
        public Nullable<int> ProductSampleId { get; set; }
        public string ProductSampleCode { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CustomerImage { get; set; }
        public string ProductImage { get; set; }
        public Nullable<int> ProductLinkId { get; set; }

    }
}
