using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwMaterialInbound
    {
        public vwMaterialInbound()
        {

        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string Code { get; set; }
        public string Note { get; set; }
        public Nullable<int> WarehouseKeeperId { get; set; }
        public string Type { get; set; }        
        public decimal? TotalAmount { get; set; }
        public Nullable<bool> IsDone { get; set; }
        public int? BranchId { get; set; }
        public int? WarehouseDestinationId { get; set; }
        public string WarehouseDestinationName { get; set; }
        public Nullable<int> PhysicalInventoryId { get; set; }
        
        public bool IsArchive { get; set; }

        public int? CreatedStaffId { get; set; }
        public string CreatedStaffName { get; set; }

        public string DistrictId { get; set; }
        public string CityId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public Nullable<int> AssignedUserId { get; set; }
        public Nullable<int> MaterialOutboundId { get; set; }
    }
}