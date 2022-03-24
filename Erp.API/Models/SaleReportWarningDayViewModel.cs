using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class SaleReportWarningDayViewModel
    {
        public decimal InvoiceTotalAmountbyCreateAndManager { get; set; } // tổng doanh thu theo nhân viên tạo, nhân viên quản lý khách hàng
        public decimal CountInvoicebyCreateAndManager { get; set; } //tổng đơn hàng trong ngày theo nhân viên tạo, nhân viên quản lý khách hàng

        public decimal InvoiceTotalAmountbyCreateOtherManager { get; set; } // tổng doanh thu theo nhân viên tạo khác nhân viên quản lý khách hàng
        public decimal CountInvoicebyCreateOtherManager { get; set; } //tổng đơn hàng trong ngày theo nhân viên tạo khác nhân viên quản lý khách hàng

        public decimal CountSchedulingHistory { get; set; } // tổng số liệu trình đã làm của user

        public string TotalDayHourMinute { get; set; } // tổng số giờ làm liệu trình đã làm của user

        //public List<SaleReportPaymentMethodViewModel> PaymentMethodInvoice { get; set; } // các phương thức thanh toán bán hàng
        //public List<SaleReportPaymentMethodViewModel> PaymentMethodPurchase { get; set; } // các phương thức thanh toán mua hàng
    }
    public class SaleReportWarningDaySPCongNoViewModel
    {
        public string MaChungTuGoc { get; set; }
        public decimal CongNo { get; set; }

    }
    public class SaleReportPaymentMethodViewModel
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
    }
}