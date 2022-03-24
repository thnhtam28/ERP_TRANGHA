using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwPhysicalInventoryMaterialDetail
    {
        public vwPhysicalInventoryMaterialDetail()
        {

        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public int PhysicalInventoryMaterialId { get; set; }
        public int MaterialId { get; set; }
        public string CategoryCode { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public int QuantityInInventory { get; set; }
        public int QuantityRemaining { get; set; }
        public int QuantityDiff { get; set; }
        public string Note { get; set; }
        public string ReferenceVoucher { get; set; }
        public string LoCode { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
    }
}
