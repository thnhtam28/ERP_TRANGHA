using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwDonateProOrSer
    {
        public vwDonateProOrSer()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        public Nullable<int> TargetId { get; set; }
        public string TargetModule { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> ExpriryMonth { get; set; }
        public Nullable<int> TotalQuantity { get; set; }
        public Nullable<int> RemainQuantity { get; set; }
        //
        public string CategoryCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal? Price { get; set; }
        public string ProductType { get; set; }
    }
}
