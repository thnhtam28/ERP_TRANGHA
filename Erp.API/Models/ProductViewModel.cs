using System;

namespace Erp.API.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal? PriceInbound { get; set; }
        public decimal? PriceOutbound { get; set; }
        public string Type { get; set; }

        public string CategoryCode { get; set; }

        public int? MinInventory { get; set; }
        public string Image_Name { get; set; }
        public string ProductGroup { get; set; }
        public string Manufacturer { get; set; }
        public string Size { get; set; }
        public int? QuantityTotalInventory { get; set; }
        public int? DiscountStaff { get; set; }
        public bool? IsMoneyDiscount { get; set; }
        public string LoCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        public int? ProductLinkId { get; set; }
        public string ProductLinkName { get; set; }
        public string ProductLinkCode { get; set; }
        public int QuantityDayUsed { get; set; }
        public int QuantityDayNotify { get; set; }
        public int ProductDetailId { get; set; }

        public string Origin { get; set; }
        public string Categories { get; set; }

    }

    public class ServiceInPackage
    {
        public int? ProductId { set; get; }
        public int? Quantity { set; get; }
    }
}