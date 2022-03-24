using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwProductDetail
    {
        public vwProductDetail()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }

        public int ProductId { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
    }
}
