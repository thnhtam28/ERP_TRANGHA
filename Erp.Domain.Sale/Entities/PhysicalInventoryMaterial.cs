﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class PhysicalInventoryMaterial
    {
        public PhysicalInventoryMaterial()
        {

        }
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public Nullable<int> WarehouseId { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public Nullable<int> BranchId { get; set; }
        public bool? IsExchange { get; set; }
    }
}
