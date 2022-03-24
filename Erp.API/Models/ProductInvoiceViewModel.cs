using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class ProductInvoiceViewModel
    {
        public ProductInvoiceViewModel()
        {
            Discount = 0;
            TaxFee = 0;
            TotalAmount = 0;
            DiscountAmount = 0;
        }

        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Code { get; set; }
        public decimal TotalAmount { get; set; }
        public double TaxFee { get; set; }
        public decimal TongTienSauVAT { get; set; }
        public double Discount { get; set; }
        public int? CustomerId { get; set; }
        public Nullable<int> CustomerDiscountId { get; set; }
        public string ShipName { get; set; }
        public string ShipPhone { get; set; }
        public string ShipAddress { get; set; }
        public string ShipWardId { get; set; }
        public string ShipDistrictId { get; set; }
        public string ShipCityId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public bool IsArchive { get; set; }
        public int? BranchId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public string CancelReason { get; set; }
        public string BarCode { get; set; }

        public int? SaleOrderId { get; set; }
        public string SaleOrderCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string ShipWardName { get; set; }
        public string ShipDistrictName { get; set; }
        public string ShipCityName { get; set; }
        public string BranchName { get; set; }
        public string ProductOutboundCode { get; set; }

        public int? ProductOutboundId { get; set; }

        public bool? IsReturn { get; set; }

        public List<ProductInvoiceDetailViewModel> DetailList { get; set; }
        public List<ProductInvoiceDetailViewModel> GroupProduct { get; set; }

        public DateTime? NextPaymentDate { get; set; }
        public DateTime? NextPaymentDate_Temp { get; set; }
        public string CodeInvoiceRed { get; set; }
        public Nullable<int> ProductItemCount { get; set; }
        public decimal? DiscountAmount { get; set; }

        public int? StaffCreateId { get; set; }

        public string StaffCreateName { get; set; }

        public bool? AllowEdit { get; set; }
        public ProductOutboundViewModel ProductOutboundViewModel { get; set; }
        public int? QuantityCodeSaleReturns { get; set; }

        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? Day { get; set; }
        public int? WeekOfYear { get; set; }
        public string UserTypeCode { get; set; }

        public string strCreatedDate { get; set; }

        public string DistrictId { get; set; }
        public string CityId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
    }


}