using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
    public class vwProductInvoice_NVKD
    {
        public vwProductInvoice_NVKD()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string Code { get; set; }
        public decimal TotalAmount { get; set; }
        public Nullable<double> TaxFee { get; set; }
        public Nullable<double> Discount { get; set; }

        public decimal? DiscountAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? RemainingAmount { get; set; }

        public string Status { get; set; }
        public string Note { get; set; }
        public bool IsArchive { get; set; }
        public int BranchId { get; set; }
        public string PaymentMethod { get; set; }
        public string CancelReason { get; set; }

        public string CustomerCode { get; set; }
        public string BranchName { get; set; }
        public string CodeInvoiceRed { get; set; }
        public Nullable<bool> IsReturn { get; set; }
        public Nullable<System.DateTime> NextPaymentDate { get; set; }
        public string StaffCreateName { get; set; }

        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string Type { get; set; }
        
        public Nullable<int> CustomerId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? Day { get; set; }
        public int? WeekOfYear { get; set; }
       
        public Nullable<bool> IsBonusSales { get; set; }
        public decimal? DoanhThu { get; set; }
        public int? ManagerStaffId { get; set; }
        public string ManagerStaffName { get; set; }
        public string ManagerUserName { get; set; }
        public decimal? tiendathu { get; set; }
        public decimal? tienconno { get; set; }

        public double? Discount_VIP { get; set; }
        public double? Discount_KM { get; set; }
        public double? Discount_DB { get; set; }
        public string CountForBrand { get; set; }
        public decimal? TotalDebit { get; set; }
        public decimal? TotalCredit { get; set; }
        public int? coMBS { get; set; }
        public string UserTypeName { get; set; }
        public int? UserTypeId { get; set; }
        public string ProductInvoiceOldCode { get; set; }
        public string GDDauTienThanhToanHet { get; set; }
        public string GDNgayDauTienThanhToanHet { get; set; }
        public string SPHangHoa { get; set; }
        public string SPDichvu { get; set; }
        public string Hangduoctang { get; set; }

        public Nullable<System.DateTime> VoucherDate { get; set; }
        public double? TyleHuong { get; set; }


    }
}
