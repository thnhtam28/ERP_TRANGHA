using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class BaoCaoBanGiaDichVuViewModel
    {
        public int? AssignedUserId { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string AssignedUserName { get; set; }
        public string ProductImage { get; set; }
        public int Id { get; set; }
        public decimal? Price { get; set; }
        public int? Percent { get; set; }
        public string AdjustmentType { get; set; }
        public int? BranchId { get; set; }
        public int? SupplierId { get; set; }
        //mả sản phẩm
        public string PurchaseOrderCode { get; set; }
        public string ProductCode { get; set; }
        //tên sản phẩm
        public string ProductName { get; set; }
        //danh mục sp
        public string CategoryCode { get; set; }
        //nhà sản xuất
        public string Manufacturer { get; set; }
        //
        public string SerialNumber { get; set; }
        public string ProductColor { get; set; }
        //số lượng
        public int? Quantity { get; set; }
        //đơn vị tính
        public string Unit { get; set; }
        public decimal? PriceService { get; set; }
        public string ServiceName { get; set; }

        public bool? IsApproved { get; set; }
        public bool? IsPayment { get; set; }
        public decimal? LaborAmount { get; set; }
        public decimal? LaborPaidAmount { get; set; }
        public decimal? LaborRemainAmount { get; set; }
        public int? TongSoLan { get; set; }
    }
}