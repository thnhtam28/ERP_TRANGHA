using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities
{
    public class vwCustomerRecommend
    {
        public vwCustomerRecommend()
        {

        }

        public int Id { get; set; }
        public string Code { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<System.DateTime> StartDate { get; set; }
        //public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        //public string Code { get; set; }
        public string CustomerName { get; set; }
   
       
        public string FullName { get; set; }
        public int CustomerId_new { get; set; }

    }
}
