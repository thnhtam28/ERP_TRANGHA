using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class QuanLyPhamTheoKhoViewModel
    {
        public int Id { get; set; }
        //id đơn hàng
        public int? PurchaseOrderId { get; set; }
        //id sản phẩm
        public int? ProductId { get; set; }
        //đơn giá
        public decimal? Price { get; set; }
        public decimal? PriceTemp { get; set; }
        //số lượng
        public int? Quantity { get; set; }
        //đơn vị tính
        public string Unit { get; set; }
        //% chiết khấu
        public int? DisCount { get; set; }
        // giá trị chiết khấu
        public int? DisCountAmount { get; set; }
        //tổng tiền
        public decimal? Amount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainAmount { get; set; }
        //mả sản phẩm
        public string ProductCode { get; set; }
        //tên sản phẩm
        public string ProductName { get; set; }
        //danh mục sp
        public string CategoryCode { get; set; }
        //nhà sản xuất
        public string Manufacturer { get; set; }
        //
        public string SerialNumber { get; set; }

        //mả đơn hàng
        public string PurchaseOrderCode { get; set; }
        //ghi sổ
        public bool? IsArchive { get; set; }
        //nhóm hàng
        public string ProductGroup { get; set; }
        //ngày nhập hàng
        public DateTime? PurchaseOrderDate { get; set; }
        //mã nhà cung cấp
        public string SupplierCode { get; set; }
        //id ncc
        public int? SupplierId { get; set; }
        //tên ncc
        public string SupplierName { get; set; }
        //kho nhập hàng
        public string WarehouseDestinationName { get; set; }
        public string WarehouseDestinationCode { get; set; }
        //chi nhánh
        public int WarehouseDestinationId { get; set; }
        public string BranchName { get; set; }
        public int? BranchId { get; set; }
        public string ProductImage { get; set; }
        public bool? IsReturned { get; set; }
        public decimal? RemainAmountInvoice { get; set; }
        public decimal? AmountInvoice { get; set; }
        public string OutboundStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string ReceiptStatus { get; set; }
    }
}