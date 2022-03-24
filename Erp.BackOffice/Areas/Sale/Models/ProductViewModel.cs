using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "IsDeleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "CreatedUser", ResourceType = typeof(Wording))]
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(Wording))]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "ModifiedUser", ResourceType = typeof(Wording))]
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }

        [Display(Name = "ModifiedDate", ResourceType = typeof(Wording))]
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [StringLength(100, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError", ErrorMessage = null)]
        [Display(Name = "ProductName", ResourceType = typeof(Wording))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "ProductCode", ResourceType = typeof(Wording))]
        public string Code { get; set; }

        [Display(Name = "Description", ResourceType = typeof(Wording))]
        public string Description { get; set; }

        [Display(Name = "Unit", ResourceType = typeof(Wording))]
        public string Unit { get; set; }

        [Display(Name = "PriceInbound", ResourceType = typeof(Wording))]
        public decimal? PriceInbound { get; set; }

        [Display(Name = "PriceOutbound", ResourceType = typeof(Wording))]
        public decimal? PriceOutbound { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Wording))]
        public string Type { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "CategoryCode", ResourceType = typeof(Wording))]
        public string CategoryCode { get; set; }

        [Display(Name = "MinInventory", ResourceType = typeof(Wording))]
        public int? MinInventory { get; set; }


        [Display(Name = "IsCombo", ResourceType = typeof(Wording))]
        public bool? IsCombo { get; set; }

        [Display(Name = "Barcode", ResourceType = typeof(Wording))]
        public string Barcode { get; set; }

        //[Display(Name = "Barcode", ResourceType = typeof(Wording))]
        public string Image_Name { get; set; }

        public List<ObjectAttributeValueViewModel> AttributeValueList { get; set; }
        public List<ProductDetailViewModel> ProductDetailList { get; set; }

        [Display(Name = "ProductGroup", ResourceType = typeof(Wording))]
        public string ProductGroup { get; set; }

        [Display(Name = "Manufacturer", ResourceType = typeof(Wording))]
        public string Manufacturer { get; set; }
        [Display(Name = "ProductCapacity", ResourceType = typeof(Wording))]
        public string Size { get; set; }
        public int? QuantityTotalInventory { get; set; }
        public int? DiscountStaff { get; set; }
        public bool? IsMoneyDiscount { get; set; }

        [Display(Name = "LoCode", ResourceType = typeof(Wording))]
        public string LoCode { get; set; }
        [Display(Name = "ExpiryDateItem", ResourceType = typeof(Wording))]
        public DateTime? ExpiryDate { get; set; }
        public int MaterialId { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "ProductLinkId", ResourceType = typeof(Wording))]
        public int? ProductLinkId { get; set; }
        [Display(Name= "ProductLinkName", ResourceType = typeof(Wording))]
        public string ProductLinkName { get; set; }
        [Display(Name ="ProductLinkCode", ResourceType =typeof(Wording))]
        public string ProductLinkCode { get; set; }

 		[Display(Name = "QuantityDayUsed", ResourceType = typeof(Wording))]
        public int QuantityDayUsed { get; set; }
        [Display(Name = "QuantityDayNotify", ResourceType = typeof(Wording))]
        public int QuantityDayNotify { get; set; }
        public int ProductDetailId { get; set; }
        [Display(Name = "Origin", ResourceType = typeof(Wording))]
        public string Origin { get; set; }

        public string EquimentGroup { get; set; }
        [Display(Name = "IsCNC", ResourceType = typeof(Wording))]
        public string IsCNC { get; set; }
        public string Categories { get; set; }

        public decimal? Amount { get; set; }
    }
}