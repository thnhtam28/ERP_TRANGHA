using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwInventoryMaterial
    {
        public vwInventoryMaterial()
        {

        }
       public int MaterialId { get; set; }
	
		public string ProductGroup { get; set; }
	
		public string CategoryCode { get; set; }
	
		public string MaterialName { get; set; }
	
		public string MaterialCode { get; set; }
	
		public string MaterialBarcode { get; set; }
	
		public string MaterialUnit { get; set; }
	
		public int? MaterialMinInventory { get; set; }
	
		public int? WarehouseId { get; set; }
	
		public string WarehouseName { get; set; }
	
		public int? BranchId { get; set; }
	
		public string BranchName { get; set; }
	
		public int Quantity { get; set; }
	
		public string LoCode { get; set; }
	
		public DateTime? ExpiryDate { get; set; }
	
		public string Image_Name { get; set; }
	
		public int? CBTK { get; set; }
	
		public bool? IsSale { get; set; }
	
		public string MaterialManufacturer { get; set; }
	
		public decimal? MaterialPriceInbound { get; set; }
	
		public decimal? MaterialPriceOutbound { get; set; }
	
	}
}
