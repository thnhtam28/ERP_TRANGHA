using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwMaterialOutboundDetail
    {
        public int Id { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUserId { get; set; }

        public int? AssignedUserId { get; set; }

        public int? MaterialOutboundId { get; set; }

        public int? MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public string Unit { get; set; }

        public string WarehouseDestinationName { get; set; }

        public string WarehouseSourceName { get; set; }

        

        public string MaterialOutboundCode { get; set; }
        public string LoCode { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public int? WarehouseDestinationId { get; set; }

        public int? WarehouseSourceId { get; set; }
        public bool IsArchive { get; set; }

        public string Type { get; set; }

    }
}
