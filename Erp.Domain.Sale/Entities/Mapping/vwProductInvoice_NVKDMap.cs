using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwProductInvoice_NVKDMap : EntityTypeConfiguration<vwProductInvoice_NVKD>
    {
        public vwProductInvoice_NVKDMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Code).HasMaxLength(20);
            this.Property(t => t.Status).HasMaxLength(50);
            this.Property(t => t.Note).HasMaxLength(300);


            // Table & Column Mappings
            this.ToTable("vwSale_ProductInvoice_NVKD");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.TaxFee).HasColumnName("TaxFee");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.DiscountAmount).HasColumnName("DiscountAmount");
            this.Property(t => t.PaidAmount).HasColumnName("PaidAmount");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsArchive).HasColumnName("IsArchive");

            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.CancelReason).HasColumnName("CancelReason");
            this.Property(t => t.PaymentMethod).HasColumnName("PaymentMethod");

            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.CodeInvoiceRed).HasColumnName("CodeInvoiceRed");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.NextPaymentDate).HasColumnName("NextPaymentDate");
            this.Property(t => t.StaffCreateName).HasColumnName("StaffCreateName");
            
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CustomerPhone).HasColumnName("CustomerPhone");
            //abc
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Day).HasColumnName("Day");
            this.Property(t => t.WeekOfYear).HasColumnName("WeekOfYear");
            this.Property(t => t.DoanhThu).HasColumnName("DoanhThu");
            this.Property(t => t.ManagerStaffId).HasColumnName("ManagerStaffId");
            this.Property(t => t.ManagerStaffName).HasColumnName("ManagerStaffName");
            this.Property(t => t.ManagerUserName).HasColumnName("ManagerUserName");
            this.Property(t => t.tiendathu).HasColumnName("tiendathu");
            this.Property(t => t.tienconno).HasColumnName("tienconno");
            this.Property(t => t.Discount_VIP).HasColumnName("Discount_VIP");
            this.Property(t => t.Discount_KM).HasColumnName("Discount_KM");
            this.Property(t => t.Discount_DB).HasColumnName("Discount_DB");
            this.Property(t => t.TotalDebit).HasColumnName("TotalDebit");
            this.Property(t => t.TotalCredit).HasColumnName("TotalCredit");
            this.Property(t => t.coMBS).HasColumnName("coMBS");
            //
            this.Property(t => t.UserTypeName).HasColumnName("UserTypeName");
            this.Property(t => t.UserTypeId).HasColumnName("UserTypeId");
            this.Property(t => t.ProductInvoiceOldCode).HasColumnName("ProductInvoiceOldCode");
            this.Property(t => t.GDDauTienThanhToanHet).HasColumnName("GDDauTienThanhToanHet");
            this.Property(t => t.GDNgayDauTienThanhToanHet).HasColumnName("GDNgayDauTienThanhToanHet");

            this.Property(t => t.SPHangHoa).HasColumnName("SPHangHoa");
            this.Property(t => t.SPDichvu).HasColumnName("SPDichvu");
            this.Property(t => t.Hangduoctang).HasColumnName("Hangduoctang");
            this.Property(t => t.VoucherDate).HasColumnName("VoucherDate");
            this.Property(t => t.TyleHuong).HasColumnName("TyleHuong");

        }
    }
}
